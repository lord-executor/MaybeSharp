using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeCreateTest
{
    [Test]
    public void MaybeOf__Value__CreatesJustValue()
    {
        var obj = new DemoType();

        var result = Maybe.Of(obj);

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(obj);
    }

    [Test]
    public void ToMaybe__FromValue__CreatesJustValue()
    {
        var obj = new DemoType();

        var result = obj.ToMaybe();

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(obj);
    }

    [Test]
    public void MaybeOf__Null__CreatesNothing()
    {
        var result = Maybe.Of<DemoType>(null);

        result.Should().NotBeNull();
        result.Should().Be(Maybe.Nothing<DemoType>());
    }

    [Test]
    public void ToMaybe__FromNull__CreatesNothing()
    {
        DemoType? obj = null;

        var result = obj.ToMaybe();

        result.Should().NotBeNull();
        result.Should().Be(Maybe.Nothing<DemoType>());
    }
}
