using System;

namespace MaybeSharp;

/// <summary>
/// Convenience operations for the <see cref="IMaybe{T}"/> monad. The extension methods provide an easy to use API
/// and some commonly used aliases over the minimalist <see cref="IMaybe{T}"/> interface.
/// </summary>
public static class MaybeExtensions
{
    /// <summary>
    /// Generalized mapping operator for the Maybe Monad. It unwraps the value, applies the computation <paramref name="just"/>
    /// or <paramref name="nothing"/> to that current value and returns the resulting IMaybe.
    /// The <paramref name="just"/> function is only called if the current instance has a value, otherwise <paramref name="nothing"/>
    /// is called.
    /// </summary>
    /// <returns>The result of applying <paramref name="just"/> to the unwrapped value or <paramref name="nothing"/> to "Nothing"</returns>
    public static IMaybe<TResult> Map<T, TResult>(this IMaybe<T> maybe, Func<T, IMaybe<TResult>> just, Func<IMaybe<TResult>> nothing)
        where T : notnull
        where TResult : notnull
    {
        return maybe.Bind(just).Default(nothing);
    }

    /// <summary>
    /// Generalized mapping operator for the Maybe Monad. It unwraps the value, applies the computation <paramref name="just"/>
    /// or <paramref name="nothing"/> to that current value and returns the resulting IMaybe.
    /// The <paramref name="just"/> function is only called if the current instance has a value, otherwise <paramref name="nothing"/>
    /// is called.
    ///
    /// This version of the "Map" method works with "degenerate" functions that don't return monadic types. The method ensures
    /// that the results of the mapping functions are properly converted to monadic types using <see cref="Maybe.Of{TResult}(TResult)"/>.
    /// </summary>
    /// <returns>The result of applying <paramref name="just"/> to the unwrapped value or <paramref name="nothing"/> to "Nothing"</returns>
    public static IMaybe<TResult> Map<T, TResult>(this IMaybe<T> maybe, Func<T, TResult?> just, Func<TResult?> nothing)
        where T : notnull
        where TResult : notnull
    {
        return maybe.Map(Lift(just), Lift(nothing));
    }

    /// <summary>
    /// This is the implementation of the canonical Monad "bind" operator. It applies the given function to the monadic variable
    /// and returns the result. If this is a "Nothing", then the binding operation has essentially no effect.
    ///
    /// This version of the "Bind" method works with "degenerate" functions that don't return monadic types. The method ensures
    /// that the results of the binding function is properly converted to a monadic type using <see cref="Maybe.Of{TResult}(TResult)"/>
    /// </summary>
    /// <returns>The result of lifting the return value of <paramref name="func"/> when applied to the unwrapped value</returns>
    public static IMaybe<TResult> Bind<T, TResult>(this IMaybe<T> maybe, Func<T, TResult?> func)
        where T : notnull
        where TResult : notnull
    {
        return maybe.Bind(Lift(func));
    }

    /// <summary>
    /// Maps any "Nothing" to the given <paramref name="defaultValue"/>.
    /// </summary>
    /// <returns>A new <see cref="IMaybe{T}"/> where "Nothing" has been mapped to a default value</returns>
    public static IMaybe<T> Default<T>(this IMaybe<T> maybe, IMaybe<T> defaultValue)
        where T : notnull
    {
        return maybe.Default(() => defaultValue);
    }

    /// <summary>
    /// Maps any "Nothing" to the given <paramref name="defaultValue"/>.
    ///
    /// A non-monadic default value is converted to a monadic type.
    /// </summary>
    /// <returns>A new <see cref="IMaybe{T}"/> where "Nothing" has been mapped to a default value</returns>
    public static IMaybe<T> Default<T>(this IMaybe<T> maybe, T? defaultValue)
        where T : notnull
    {
        return maybe.Default(Maybe.Of(defaultValue));
    }

    /// <summary>
    /// The Do method is the Maybe Monad equivalent of an if-else statement. It relies on side effects of the given
    /// <paramref name="just"/> and <paramref name="nothing"/> actions and is a concession to an OO language where
    /// side effects are omnipresent. It works just like the "Map" method but does not return anything.
    /// </summary>
    public static void Do<T>(this IMaybe<T> maybe, Action<T> just, Action nothing)
        where T : notnull
    {
        maybe.Map(Void(just), Void<T>(nothing));
    }

    /// <summary>
    /// The Do method is the Maybe Monad equivalent of an if statement. It relies on side effects of the given
    /// <paramref name="just"/> action and is a concession to an OO language where side effects are omnipresent.
    /// It works just like the <see cref="Bind{T, TResult}(IMaybe{T}, Func{T, TResult})"/> method
    /// but does not return anything.
    /// </summary>
    public static void Do<T>(this IMaybe<T> maybe, Action<T> just)
        where T : notnull
    {
        maybe.Map(Void(just), NoOpNothing<T>);
    }

    /// <summary>
    /// The Do method is the Maybe Monad equivalent of an if statement. It relies on side effects of the given
    /// <paramref name="nothing"/> action and is a concession to an OO language where side effects are omnipresent.
    /// This overload only accepts an action for the "Nothing" case that is executed if the instance is "Nothing".
    /// </summary>
    public static void Do<T>(this IMaybe<T> maybe, Action nothing)
        where T : notnull
    {
        maybe.Map(NoOpJust, Void<T>(nothing));
    }


    #region Private Type Conversion Functions

    /// <summary>
    /// Simple no-op transformation for "Nothing"
    /// </summary>
    private static IMaybe<T> NoOpNothing<T>()
        where T : notnull
    {
        return Maybe.Nothing<T>();
    }

    /// <summary>
    /// Simple no-op transformation for "Just"
    /// </summary>
    private static IMaybe<T> NoOpJust<T>(T value)
        where T : notnull
    {
        return Maybe.Of(value);
    }

    /// <summary>
    /// An operator that "lifts" a degenerated "Just" mapping function up to be a proper monadic mapping function.
    /// </summary>
    private static Func<T, IMaybe<TResult>> Lift<T, TResult>(Func<T, TResult?> func)
        where TResult : notnull
    {
        return v => Maybe.Of(func(v));
    }

    /// <summary>
    /// An operator that "lifts" a degenerated "Nothing" mapping function up to be a proper monadic mapping function.
    /// </summary>
    private static Func<IMaybe<TResult>> Lift<TResult>(Func<TResult?> func)
        where TResult : notnull
    {
        return () => Maybe.Of(func());
    }

    /// <summary>
    /// An operator that converts a "Just" action into a proper monadic function.
    /// </summary>
    private static Func<T, IMaybe<T>> Void<T>(Action<T> action)
        where T : notnull
    {
        return v =>
        {
            action(v);
            return NoOpJust(v);
        };
    }

    /// <summary>
    /// An operator that converts a "Nothing" action into a proper monadic function.
    /// </summary>
    private static Func<IMaybe<T>> Void<T>(Action action)
        where T : notnull
    {
        return () =>
        {
            action();
            return NoOpNothing<T>();
        };
    }

    #endregion
}
