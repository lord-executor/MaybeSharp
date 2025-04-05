using System;

namespace MaybeSharp;

/// <summary>
/// This is the minimal Monad interface. Additional "convenience" functionality is provided by the <see cref="MaybeExtensions"/>
/// </summary>
/// <typeparam name="T">Underlying (nullable / reference) type</typeparam>
public interface IMaybe<T> : IEquatable<IMaybe<T>> where T : notnull
{
    /// <summary>
    /// This is the implementation of the canonical Monad "bind" operator. It applies the given function to the monadic variable
    /// and returns the result. If this is a "Nothing", then the binding operation has essentially no effect.
    /// </summary>
    /// <returns>The result of applying the binding <paramref name="just"/> to the monadic variable</returns>
    IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> just) where TResult : notnull;

    /// <summary>
    /// Maps any "Nothing" to the given <paramref name="defaultValue"/>.
    /// </summary>
    /// <returns>A new <see cref="IMaybe{T}"/> where "Nothing" has been mapped to the result of the default value function</returns>
    IMaybe<T> Default(Func<IMaybe<T>> defaultValue);

    /// <summary>
    /// Unwraps and returns the underlying value, bringing it back into the world of nulls. A <paramref name="defaultValue"/> can
    /// be provided that will be returned if the current instance is "Nothing".
    /// </summary>
    /// <returns>The underlying value if this is a "Just"; null or the provided default value if this is a "Nothing"</returns>
    T? Extract(T? defaultValue = default);
}
