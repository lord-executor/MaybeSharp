using System;

namespace MaybeSharp
{
	/// <summary>
	/// Convenience operations for the <see cref="IMaybe{T}"/> monad. The extension methods provide an easy to use API
	/// over the minimalist <see cref="IMaybe{T}"/> interface.
	/// </summary>
	public static class MaybeExtensions
	{
		private static Action NothingNoOp = () => { };

		/// <summary>
		/// Convenience method that is a degenerated version of the <see cref="IMaybe{T}.Bind{TResult}(Func{T, IMaybe{TResult}})"/>
		/// operator that automatically projects the non-Maybe result of the function into the Monad by invoking the unit function
		/// on it.
		/// </summary>
		/// <returns>The result of lifting the return value of <paramref name="func"/> when applied to the unwrapped value</returns>
		public static IMaybe<TResult> Bind<T, TResult>(this IMaybe<T> maybe, Func<T, TResult> func)
			where T : class
			where TResult : class
		{
			return maybe.Bind(v => Maybe.Of(func(v)));
		}

		/// <summary>
		/// Convenience method for only appling a "just" action to the maybe object.
		/// </summary>
		public static void Do<T>(this IMaybe<T> maybe, Action<T> just)
			where T : class
		{
			maybe.Do(just, NothingNoOp);
		}
	}
}
