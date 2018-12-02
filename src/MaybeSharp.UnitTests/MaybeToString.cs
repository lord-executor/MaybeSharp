using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeToString
	{
		[Test]
		public void ToString__OnJust__ContainsTypeAndValue()
		{
			var obj = new DemoType("John");

			var result = Maybe.Of(obj).ToString();

			result.Should().NotBeNull();
			result.Should().Contain("Just");
			result.Should().Contain(typeof(DemoType).Name);
			result.Should().Contain(obj.Name);
		}

		[Test]
		public void ToString__OnNothing__ContainsType()
		{
			var result = Maybe.Of<DemoType>(null).ToString();

			result.Should().NotBeNull();
			result.Should().Contain("Nothing");
			result.Should().Contain(typeof(DemoType).Name);
		}
	}
}
