[![Build Status](https://travis-ci.org/lord-executor/MaybeSharp.svg?branch=master)](https://travis-ci.org/lord-executor/MaybeSharp)

# Overview
This is a C# implementation of the "Maybe" monad concept from functional programming. If you don't know yet what a monad is or what the maybe monad is, have a look at some of the links at the bottom.

There are many approaches to implement a functional concept like the maybe monad in an object oriented language. This implementation has some key distinguishing features over some of the alternatives you might find:
* It has a convenient API that does not require a lot of know-how to use properly.
* It is actually useful and can be used in production applications to help deal with the uncertainties of null values.
* It respects the object oriented nature of C#.
* Fully unit tested (100% code coverage).

Note to pedants (like myself):
Yes, I am aware that with this implementation it is possible to pass a null value to a method that expects an instance of `IMaybe<T>`. The point of this API is to make null handling more explicit, not idiot proof - there is always a bigger idiot, for example the developer that decides to use null as the value for an `IMaybe<T>`.



# Using the Maybe Monad
Rule #1: Never ever assign null to a variable or parameter of type `IMaybe<T>` and never ever leave a variable of that type unassigned! Instead of assigning null, you should **always** initialize maybes with a proper value using `Maybe.Of(value)` or `Maybe.Nothing<T>()` if you don't have a value.

That's it. Besides this one and really, really important rule there is not much you have to look out for.

## Basic Operations

The first and most important operation is the **unit** or **return** type converter called `Maybe.Of<T>`.
```cs
var maybe = Maybe.Of(someVariableThatMayOrMayNotBeNull);
var maybeJust = Maybe.Of(new Object()); // this is a "Just"
var maybeNothing = Maybe.Of<object>(null); // this is a "Nothing"
```

Once you have a maybe instance you can start using its **bind** operator that is functionally very similar to the `?.` (null-conditional) operator in C#. It can be used to safely access properties and methods of an object.
```cs
IMaybe<string> name = Maybe.Of(person).Bind(p => p.Name);
```

Binding only has an effect on "Just" values, a "Nothing" will always remain nothing. That is where the `Default` method comes in handy:
```cs
var dummyPerson = new Person("John");
IMaybe<string> name = Maybe.Of(person).Default(dummyPerson).Bind(p => p.Name);
// if person was null, then name is now "Just 'John'"
// if person was not null, then name is now either "Nothing" (person.Name was null) or "Just person.Name"
```

Eventually, most code using the maybe monad will have to interact with "old school" C# code that doesn't work with maybes. For that purpose, the underlying monad value can be extracted with
```cs
IMaybe<Person> m = Maybe.Of(person).Bind(p => p.Spouse);
Person spouse = m.Extract(); // spouse is now back in the world of nulls
var dummy = new Person("dummy");
spouse = m.Extract(dummy); // spouse is now going to be dummy if the original person didn't have a spouse
```

## Advanced / Convenience Functionality
The core `IMaybe<T>` interface has been kept deliberately small and simple and only provides the most basic functionality. Even though there are only two implementations of the interface ("Just" and "Nothing"), the slim interface makes a lot of sense because all the convenient overloads and advanced functionality can be (and is) implemented with extension methods on `IMaybe<T>`.

### Do
Of course, a common scenario in _real_ application is to "do" something with a value. If that value is a reference type, then that most likely means a null-check with an if or if/else. With the functional approach of the maybe monad, this can be written like so:
```cs
public static void Print(IMaybe<Person> person)
{
    person.Bind(p => p.Name).Do(name => {
        Console.WriteLine("Person: {0}", name);
    });

    person.Bind(p => p.Spouse).Do(spouse => {
        Console.WriteLine("Has a spouse");
    }, () => {
        Console.WriteLine("Does not have a spouse");
    });
}
```

**Note**: The code above actually does not perform a _single_ null check. It is purely based on virtual method calls.

### Map
The `Map` method is baiscally just a combination of the `Bind` and `Default` methods. It allows the developer to provide a value transformation for both cases ("Just" and "Nothing") where only the relevant of the two functions is actually executed.
```cs
public static void Foo(IMaybe<Person> person, IMaybe<Person> guardian)
{
    var guardianName = person.Bind(p => p.Parent).Map(
        parent => Maybe.Of(parent.Name),
        () => guardian.Bind(g => g.Name)
    );

    // this is of course equivalent to the more readable ...

    guardianName = person
        .Bind(p => p.Parent)
        .Default(guardian)
        .Bind(p => p.Name);

    // ... but depending on the complexity of the transformations, Map might be the
    // better option.
}
```

## Need More?
Just have a look at the source code. It only consists of three files that are easy enough to read and understand.



# Design Considerations
This section explains some of the design considerations behind this particular implementation.

## Why not Structs
Structs being value types have the obvious advantage that you would not have to worry about maybe instances themselves being null. The problem is that structs of course have their own drawbacks because they always have to provide a parameterless public constructor which makes it very hard and awkward to implement a reasonable API. While it would solve the problem of 
* It is not possible to have a "union" type in C# that can be one of two structs, so
  * The type of maybe variables would either still have to be `IMaybe<T>` which would be nullable, or
  * Just and Nothing would have to be implemented in a single struct somehow.
* It would still be possible to misuse the struct(s) since they must have a parameterless constructor and creating a Just _requires_ a value.

## Type Guards
In functional programming, functions tend to deal with maybe by using type guards. An example for this from the Haskell documentation:
```
zeroAsDefault :: Maybe Int -> Int
zeroAsDefault mx = case mx of
    Nothing -> 0
    Just x -> x
```

This is quite normal in functional languages since type guards are a core concept of the language itself, but in an object oriented language like C# they are a bit out of place. This is why the actual Just and Nothing implementations are hidden as private classes and the `Default` and `Map` methods were added to provide a more object oriented alternative to type guards. `Map` is simply using `Bind` and `Default` under the hood and `Bind` and `Default` together allow the same functionality as type guards.

# Performance
The performance difference between using the maybe monad vs. regular C# null-conditional and null-coalescing operators is very small, but surprisingly, the maybe monad version of the code is consistently faster!
TODO: More details

# Links

* [Monad (functional programming)](https://en.wikipedia.org/wiki/Monad_(functional_programming)), Wikipedia
* [Understanding monads](https://en.wikibooks.org/wiki/Haskell/Understanding_monads), Haskell Wikibook
* [Understanding monads/Maybe](https://en.wikibooks.org/wiki/Haskell/Understanding_monads/Maybe), Haskell Wikibook
* [Monads explained in C#](https://mikhail.io/2016/01/monads-explained-in-csharp/), Mikhail Shilkov (Blog)
* [Monads explained in C# (again)](https://mikhail.io/2018/07/monads-explained-in-csharp-again/), Mikhail Shilkov (Blog)
* [Monads in C#-5. Maybe ;)](http://mikehadlow.blogspot.com/2011/01/monads-in-c-5-maybe.html), Code Rant
