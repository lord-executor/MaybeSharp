using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeEqualsTest
	{
		[Test]
		public void Equals__JustWithSameInstance__IsTrue()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);

			var result = maybe.Equals(maybe);

			result.Should().BeTrue();
		}

		[Test]
		public void Equals__JustWithSameObject__IsTrue()
		{
			var obj = new DemoType();

			var result = Maybe.Of(obj).Equals(Maybe.Of(obj));

			result.Should().BeTrue();
		}

		[Test]
		public void Equals__JustWithNull__IsFalse()
		{
			var obj = new DemoType();

			var result = Maybe.Of(obj).Equals(null);

			result.Should().BeFalse();
		}

		[Test]
		public void Equals__JustWithNothing__IsFalse()
		{
			var obj = new DemoType();

			var result = Maybe.Of(obj).Equals(Maybe.Nothing<DemoType>());

			result.Should().BeFalse();
		}

		[Test]
		public void Equals__NothingWithNothing__IsTrue()
		{
			var result = Maybe.Nothing<DemoType>().Equals(Maybe.Nothing<DemoType>());

			result.Should().BeTrue();
		}

		[Test]
		public void Equals__NothingWithOtherNothing__IsFalse()
		{
			var result = Maybe.Nothing<DemoType>().Equals(Maybe.Nothing<object>());

			result.Should().BeFalse();
		}

		[Test]
		public void Equals__NothingWithNull__IsFalse()
		{
			var result = Maybe.Nothing<DemoType>().Equals(null);

			result.Should().BeFalse();
		}
	}
}
