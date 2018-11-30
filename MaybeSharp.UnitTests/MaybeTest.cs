using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeTest
	{
		[Test]
		public void MaybeOfNullValue__Extract__ReturnsNull()
		{
			var maybe = Maybe.Of<DemoType>(null);

			var result = maybe.Extract();

			result.Should().BeNull();
		}

		[Test]
		public void MaybeOfObject__Extract__ReturnsSameReference()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);

			var result = maybe.Extract();

			result.Should().BeSameAs(obj);
		}

		[Test]
		public void MaybeOfNullValue__Do__CallsNothingAction()
		{
			var maybe = Maybe.Of<DemoType>(null);
			var sentry = new DoSentry<DemoType>();

			maybe.Do(sentry.JustAction, sentry.NothingAction);

			sentry.VerifyNothingCalled();
		}

		[Test]
		public void MaybeOfObject__Do__CallsJustAction()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);
			var sentry = new DoSentry<DemoType>();

			maybe.Do(sentry.JustAction, sentry.NothingAction);

			sentry.VerifyJustCalled(obj);
		}

		[Test]
		public void MaybeOfNullValue__Bind__DoesNotCallBinding()
		{
			var sentry = false;
			var maybe = Maybe.Of<DemoType>(null);

			var result = maybe.Bind(d => {
				sentry = true;
				return d;
			});

			sentry.Should().BeFalse();
			result.Should().NotBeNull();
			result.Extract().Should().BeNull();
		}

		[Test]
		public void MaybeOfObject__Bind__CallsBinding()
		{
			var sentry = false;
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);

			var result = maybe.Bind(d => {
				sentry = true;
				return d;
			});

			sentry.Should().BeTrue();
			result.Should().NotBeNull();
			result.Extract().Should().BeSameAs(obj);
		}

		[Test]
		public void MaybeOfObject__BindWithNewMaybe__ReturnsNewMaybe()
		{
			var maybe = Maybe.Of(new DemoType());
			var newMaybe = Maybe.Of(new DemoType());

			var result = maybe.Bind(d => newMaybe);

			result.Should().NotBeNull();
			result.Should().BeSameAs(newMaybe);
		}
	}
}
