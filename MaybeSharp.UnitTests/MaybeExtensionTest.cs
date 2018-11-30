using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeExtensionTest
	{
		[Test]
		public void Bind__OnTargetProperty__LiftsResult()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);

			var result = maybe.Bind(x => x.Name);

			result.Should().NotBeNull();
			result.Extract().Should().BeSameAs(obj.Name);
		}

		[Test]
		public void Bind__OnTargetPropertyWithNullValue__LiftsResult()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);

			var result = maybe.Bind(x => x.Optional);

			result.Should().NotBeNull();
			result.Extract().Should().BeNull();
		}

		[Test]
		public void Do__OnNothing__DoesNotCallJustAction()
		{
			var maybe = Maybe.Of<DemoType>(null);
			var sentry = new DoSentry<DemoType>();

			maybe.Do(sentry.JustAction);

			sentry.VerifyJustNotCalled();
		}

		[Test]
		public void Do__OnObject__CallsJustAction()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);
			var sentry = new DoSentry<DemoType>();

			maybe.Do(sentry.JustAction);

			sentry.VerifyJustCalled(obj);
		}
	}
}
