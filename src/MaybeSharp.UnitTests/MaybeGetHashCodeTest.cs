using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeGetHashCodeTest
{
    [Test]
    public void GetHashCode__WithSameObject__IsTrue()
    {
        var obj = new DemoType();

        var first = Maybe.Of(obj).GetHashCode();
        var second = Maybe.Of(obj).GetHashCode();

        first.Should().Be(second);
    }

    [Test]
    public void GetHashCode__WithNothing__IsStable()
    {
        var first = Maybe.Nothing<DemoType>().GetHashCode();
        var second = Maybe.Of<DemoType>(null).GetHashCode();

        first.Should().Be(second);
    }

    [Test]
    public void GetHashCode__WithDifferentNothings__AreDifferent()
    {
        var first = Maybe.Of<DemoType>(null).GetHashCode();
        var second = Maybe.Of<object>(null).GetHashCode();

        first.Should().NotBe(second);
    }
}
