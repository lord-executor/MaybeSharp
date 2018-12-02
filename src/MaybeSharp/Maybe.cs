using System;

namespace MaybeSharp
{
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
		public static IMaybe<T> Of<T>(T value)
			where T : class
		{
			// this is one of only two places that actually checks for NULL in the Maybe monad construction.
			// the other one is in the JustImpl constructor and is mostly a safety precaution against misuses
			// of the Just method.
			return value == null ? (IMaybe<T>)NothingImpl<T>.Inst : new JustImpl<T>(value);
		}

		/// <summary>
		/// Creates a "Just" object from the given value (value may NOT be null). This method is mostly here for symmetry
		/// with <see cref="Maybe.Nothing{T}"/> but should rarely to never be used directly. Use the safer
		/// <see cref="Maybe.Of{T}(T)"/> instead.
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null</exception>
		public static IMaybe<T> Just<T>(T value)
			where T : class
		{
			return new JustImpl<T>(value);
		}

		/// <summary>
		/// Creates a "Nothing" object of the underlying type <typeparamref name="T"/>.
		/// </summary>
		public static IMaybe<T> Nothing<T>()
			where T : class
		{
			return NothingImpl<T>.Inst;
		}

		/// <summary>
		/// Internal implementation of the "Just" concept
		/// </summary>
		private class JustImpl<T> : IMaybe<T>
			where T : class
		{
			private readonly T _value;

			public JustImpl(T value)
			{
				_value = value ?? throw new ArgumentException("You cannot create a 'Just' with null. Use Maybe.Of if you don't know whether the value is null or not.", nameof(value));
			}

			public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> just) where TResult : class
			{
				return just(_value);
			}

			public IMaybe<T> Default(Func<IMaybe<T>> defaultValue)
			{
				return this;
			}

			public T Extract(T defaultValue)
			{
				return _value;
			}
		}

		/// <summary>
		/// Internal implementation of the "Nothing" concept
		/// </summary>
		private class NothingImpl<T> : IMaybe<T>
				where T : class
		{
			private NothingImpl()
			{
			}

			public IMaybe<TResult> Bind<TResult>(Func<T, IMaybe<TResult>> just) where TResult : class
			{
				return Nothing<TResult>();
			}

			public IMaybe<T> Default(Func<IMaybe<T>> defaultValue)
			{
				return defaultValue();
			}

			public T Extract(T defaultValue)
			{
				return defaultValue;
			}

			/// <summary>
			/// Singleton "Nothing" instance for type <typeparamref name="T"/>
			/// </summary>
			public static NothingImpl<T> Inst { get; } = new NothingImpl<T>();
		}
	}
}
