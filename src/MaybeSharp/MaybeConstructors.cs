﻿using System.Collections.Generic;
using System.Linq;

namespace MaybeSharp
{
	/// <summary>
	/// This static class contains extension methods to create <see cref="IMaybe{T}"/> instances
	/// from other types.
	/// </summary>
	public static class MaybeConstructors
	{
		/// <summary>
		/// Alternative "unit" function that creates a maybe from any (typed) reference <paramref name="value"/>.
		/// </summary>
		/// <returns>A proper <see cref="IMaybe{T}"/> from the given value</returns>
		public static IMaybe<T> ToMaybe<T>(this T value)
			where T : class
		{
			return Maybe.Of(value);
		}

		/// <summary>
		/// Maybe version of <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>.
		/// </summary>
		/// <returns>Maybe of the first element in the collection.</returns>
		public static IMaybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable)
			where T : class
		{
			return enumerable.FirstOrDefault().ToMaybe();
		}

		/// <summary>
		/// Maybe version of <see cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>.
		/// </summary>
		/// <returns>Maybe of the last element in the collection.</returns>
		public static IMaybe<T> MaybeLast<T>(this IEnumerable<T> enumerable)
			where T : class
		{
			return enumerable.LastOrDefault().ToMaybe();
		}

		/// <summary>
		/// Maybe version of <see cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource})"/>.
		/// </summary>
		/// <returns>Maybe of the only element in the collection.</returns>
		public static IMaybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable)
			where T : class
		{
			return enumerable.SingleOrDefault().ToMaybe();
		}
	}
}
