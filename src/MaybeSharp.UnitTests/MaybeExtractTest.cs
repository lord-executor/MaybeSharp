using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeExtractTest
{
    [Test]
    public void MaybeOfNullValue__Extract__ReturnsNull()
    {
        var maybe = Maybe.Of<DemoType>(null);

        var result = maybe.Extract();

        result.Should().BeNull();
    }

    [Test]
    public void MaybeOfNullValue__ExtractWithDefault__ReturnsDefault()
    {
        var @default = new DemoType();
        var maybe = Maybe.Of<DemoType>(null);

        var result = maybe.Extract(@default);

        result.Should().NotBeNull();
        result.Should().BeSameAs(@default);
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
    public void MaybeOfObject__ExtractWithDefault__ReturnsSameReference()
    {
        var @default = new DemoType();
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);

        var result = maybe.Extract(@default);

        result.Should().BeSameAs(obj);
    }
}
