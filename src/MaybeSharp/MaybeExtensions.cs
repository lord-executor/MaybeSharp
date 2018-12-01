using System;

namespace MaybeSharp
{
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
		/// 
		/// This version of the <see cref="IMaybe{T}.Map{TResult}(Func{T, IMaybe{TResult}}, Func{IMaybe{TResult}})"/> method works
		/// with "degenerate" functions that don't return monadic types. The method ensures that the results of the mapping
		/// functions are properly converted to monadic types using <see cref="Maybe.Of{TResult}(TResult)"/>.
		/// </summary>
		/// <returns>The result of applying <paramref name="just"/> to the unwrapped value or <paramref name="nothing"/> to "Nothing"</returns>
		public static IMaybe<TResult> Map<T, TResult>(this IMaybe<T> maybe, Func<T, TResult> func, Func<TResult> nothing)
			where T : class
			where TResult : class
		{
			return maybe.Map(Lift(func), Lift(nothing));
		}

		/// <summary>
		/// This is the implementation of the canonical Monad "bind" operator. It applies the given function to the monadic variable
		/// and returns the result. If this is a "Nothing", then the binding operation has essentially no effect.
		/// </summary>
		/// <returns>The result of applying the binding <paramref name="func"/> to the monadic variable</returns>
		public static IMaybe<TResult> Bind<T, TResult>(this IMaybe<T> maybe, Func<T, IMaybe<TResult>> func)
			where T : class
			where TResult : class
		{
			return maybe.Map(func, NoOpTransform<TResult>);
		}

		/// <summary>
		/// This is the implementation of the canonical Monad "bind" operator. It applies the given function to the monadic variable
		/// and returns the result. If this is a "Nothing", then the binding operation has essentially no effect.
		/// 
		/// This version of the <see cref="Bind{T, TResult}(IMaybe{T}, Func{T, IMaybe{TResult}})"/> method works with "degenerate"
		/// functions that don't return monadic types. The method ensures that the results of the binding function is properly
		/// converted to a monadic type using <see cref="Maybe.Of{TResult}(TResult)"/>
		/// </summary>
		/// <returns>The result of lifting the return value of <paramref name="func"/> when applied to the unwrapped value</returns>
		public static IMaybe<TResult> Bind<T, TResult>(this IMaybe<T> maybe, Func<T, TResult> func)
			where T : class
			where TResult : class
		{
			return maybe.Bind(Lift(func));
		}

		/// <summary>
		/// The Do method is the Maybe Monad equivalent of an if-else statement. It relies on side-effects of the given
		/// <paramref name="just"/> and <paramref name="nothing"/> actions and is a concession to an OO language where
		/// side-effects are omnipresent. It works just like the <see cref="IMaybe{T}.Map{TResult}(Func{T, IMaybe{TResult}}, Func{IMaybe{TResult}})"/>
		/// method but does not return anything.
		/// </summary>
		public static void Do<T>(this IMaybe<T> maybe, Action<T> just, Action nothing)
			where T : class
		{
			maybe.Map(Void(just), Void<T>(nothing));
		}

		/// <summary>
		/// The Do method is the Maybe Monad equivalent of an if statement. It relies on side-effects of the given
		/// <paramref name="just"/> action and is a concession to an OO language where side-effects are omnipresent.
		/// It works just like the <see cref="Bind{T, TResult}(IMaybe{T}, Func{T, IMaybe{TResult}})"/> method
		/// but does not return anything.
		/// </summary>
		public static void Do<T>(this IMaybe<T> maybe, Action<T> just)
			where T : class
		{
			maybe.Map(Void(just), NoOpTransform<T>);
		}


		#region Private Type Conversion Functions

		/// <summary>
		/// Mapping function for "Nothing" - converting from one type of "Nothing" to another.
		/// </summary>
		private static IMaybe<TResult> NoOpTransform<TResult>()
			where TResult : class
		{
			return Maybe.Nothing<TResult>();
		}

		/// <summary>
		/// An operator that "lifts" a degenerated "Just" mapping function up to be a proper monadic mapping function.
		/// </summary>
		private static Func<T, IMaybe<TResult>> Lift<T, TResult>(Func<T, TResult> func)
			where TResult : class
		{
			return v => Maybe.Of(func(v));
		}

		/// <summary>
		/// An operator that "lifts" a degenerated "Nothing" mapping function up to be a proper monadic mapping function.
		/// </summary>
		private static Func<IMaybe<TResult>> Lift<TResult>(Func<TResult> func)
			where TResult : class
		{
			return () => Maybe.Of(func());
		}

		/// <summary>
		/// An operator that converts a "Just" action into a proper monadic function.
		/// </summary>
		private static Func<T, IMaybe<T>> Void<T>(Action<T> action)
			where T : class
		{
			return v =>
			{
				action(v);
				return Maybe.Nothing<T>();
			};
		}

		/// <summary>
		/// An operator that converts a "Nothing" action into a proper monadic function.
		/// </summary>
		private static Func<IMaybe<T>> Void<T>(Action action)
			where T : class
		{
			return () =>
			{
				action();
				return Maybe.Nothing<T>();
			};
		}

		#endregion
	}
}
