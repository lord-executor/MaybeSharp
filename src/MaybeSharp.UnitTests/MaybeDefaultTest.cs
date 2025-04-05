using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeDefaultTest
{
    [Test]
    public void Default__FromNothing__ReturnsJust()
    {
        var @default = new DemoType();
        var maybe = Maybe.Of<DemoType>(null);

        var result = maybe.Default(Maybe.Of(@default));

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(@default);
    }

    [Test]
    public void Default__FromJust__ReturnsOriginal()
    {
        var @default = new DemoType();
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);

        var result = maybe.Default(Maybe.Of(@default));

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(obj);
    }

    [Test]
    public void Default__WithNonMonadicType__CreatesMonadicType()
    {
        var @default = new DemoType();
        var maybe = Maybe.Of<DemoType>(null);

        var result = maybe.Default(@default);

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(@default);
    }
}
