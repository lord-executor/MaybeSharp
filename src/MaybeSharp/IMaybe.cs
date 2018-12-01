using System;

namespace MaybeSharp
{
	/// <summary>
	/// This is the minimal Monad interface. Additional "convenience" functionality is provided by the <see cref="MaybeExtensions"/>
	/// </summary>
	/// <typeparam name="T">Underlying (nullable / reference) type</typeparam>
	public interface IMaybe<T> where T : class
	{
		/// <summary>
		/// Generalized mapping operator for the Maybe Monad. It unwraps the value, applies the computation <paramref name="just"/>
		/// or <paramref name="nothing"/> to that current value and returns the resulting IMaybe.
		/// The <paramref name="just"/> function is only called if the current instance has a value, otherwise <paramref name="nothing"/>
		/// is called.
		/// </summary>
		/// <returns>The result of applying <paramref name="just"/> to the unwrapped value or <paramref name="nothing"/> to "Nothing"</returns>
		IMaybe<TResult> Map<TResult>(Func<T, IMaybe<TResult>> just, Func<IMaybe<TResult>> nothing) where TResult : class;
		
		/// <summary>
		/// Unwraps and returns the underlying value, bringing it back into the world of nulls. A <paramref name="defaultValue"/> can
		/// be provided that will be returned if the current instance is "Nothing".
		/// </summary>
		/// <returns>The underlying value if this is a "Just"; null or the provided default value if this is a "Nothing"</returns>
		T Extract(T defaultValue = null);
	}
}
