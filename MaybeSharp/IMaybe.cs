using System;

namespace MaybeSharp
{
	/// <summary>
	/// This is the minimal Monad interface. Additional "convenience" functionality is provided by the <see cref="MaybeExtensions"/>
	/// </summary>
	/// <typeparam name="T">Monadic type</typeparam>
	public interface IMaybe<T> where T : class
	{
		/// <summary>
		/// Monad "bind" operator. It unwraps the value, applies the computation <paramref name="func"/> to the current value
		/// and returns the resulting IMaybe. The binding function is only called if the current instance has a value, otherwise
		/// it will simply return a "Nothing" of <typeparamref name="TResult"/>.
		/// </summary>
		/// <returns>The result of applying <paramref name="func"/> to the unwrapped value or "Nothing" of <typeparamref name="TResult"/>
		/// if there is no value.</returns>
		IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> func) where TResult : class;
		
		/// <summary>
		/// Functional branching depending on the underlying value. If there is a value, <paramref name="just"/> is called, otherwise
		/// <paramref name="nothing"/> is called.
		/// </summary>
		void Do(Action<T> just, Action nothing);
		
		/// <summary>
		/// Unwraps and returns the underlying value, bringing it back into the world of nulls.
		/// </summary>
		/// <returns>The underlying value if this is a "Just" or null if this is a "Nothing"</returns>
		T Extract();
	}
}
