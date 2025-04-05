using System;

namespace MaybeSharp;

/// <summary>
/// This static class contains the actual implementation of "Just" and "Nothing" as well as some static methods like
/// the "unit" type converter <see cref="Maybe.Of{T}(T)"/>. The implementations of "Just" (<see cref="Maybe.JustImpl{T}"/>)
/// and "Nothing" (<see cref="Maybe.NothingImpl{T}"/>) are hidden from the outside world as an implementation detail.
///
/// Strictly speaking, the only method that is really _required_ is for clients to make use of the Maybe Monad is the
/// <see cref="Maybe.Of{T}(T)"/> type converter.
/// </summary>
public static class Maybe
{
    /// <summary>
    /// The unit function that injects a value from an underlying type to a value in the Maybe monad. Also often
    /// referred to as "type converter" or "return". In OO-speak, this is the constructor of the Monad.
    /// </summary>
    public static IMaybe<T> Of<T>(T? value)
        where T : notnull
    {
        // this is one of only two places that actually checks for NULL in the Maybe monad construction.
        // the other one is in the JustImpl constructor and is mostly a safety precaution against misuses
        // of the Just method.
        return value == null ? NothingImpl<T>.Inst : new JustImpl<T>(value);
    }

    /// <summary>
    /// Creates a "Just" object from the given value (value may NOT be null). This method is mostly here for symmetry
    /// with <see cref="Maybe.Nothing{T}"/> but should rarely to never be used directly. Use the safer
    /// <see cref="Maybe.Of{T}(T)"/> instead.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null</exception>
    public static IMaybe<T> Just<T>(T value)
        where T : notnull
    {
        return new JustImpl<T>(value ?? throw new ArgumentException("You cannot create a 'Just' with null. Use Maybe.Of if you don't know whether the value is null or not.", nameof(value)));
    }

    /// <summary>
    /// Creates a "Nothing" object of the underlying type <typeparamref name="T"/>.
    /// </summary>
    public static IMaybe<T> Nothing<T>()
        where T : notnull
    {
        return NothingImpl<T>.Inst;
    }

    /// <summary>
    /// Internal implementation of the "Just" concept
    /// </summary>
    private sealed record JustImpl<T>(T Value) : IMaybe<T>
        where T : notnull
    {
        public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> just) where TResult : notnull
        {
            return just(Value);
        }

        public IMaybe<T> Default(Func<IMaybe<T>> defaultValue)
        {
            return this;
        }

        public T Extract(T? defaultValue)
        {
            return Value;
        }

        public override string ToString()
        {
            return $"Just<{typeof(T).Name}> \"{Value}\"";
        }

        public bool Equals(IMaybe<T>? other)
        {
            var justOther = other as JustImpl<T>;
            return this == justOther;
        }
    }

    /// <summary>
    /// Internal implementation of the "Nothing" concept
    /// </summary>
    private record NothingImpl<T> : IMaybe<T>
        where T : notnull
    {

        public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> just) where TResult : notnull
        {
            return NothingImpl<TResult>.Inst;
        }

        public IMaybe<T> Default(Func<IMaybe<T>> defaultValue)
        {
            return defaultValue();
        }

        public T? Extract(T? defaultValue)
        {
            return defaultValue;
        }

        public bool Equals(IMaybe<T>? other)
        {
            var nothingOther = other as NothingImpl<T>;
            return this == nothingOther;
        }

        public override string ToString()
        {
            return $"Nothing<{typeof(T).Name}>";
        }

        /// <summary>
        /// Singleton "Nothing" instance for type <typeparamref name="T"/>
        /// </summary>
        public static NothingImpl<T> Inst { get; } = new NothingImpl<T>();
    }
}
