using FluentAssertions;
using NUnit.Framework;
using System;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeJustTest
{
    [Test]
    public void Just__WithNull__ThrowsArgumentException()
    {
        Action action = () =>
        {
            Maybe.Just<DemoType>(null!);
        };

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Just__WithValue__CreatesMaybeJust()
    {
        var obj = new DemoType();

        var result = Maybe.Just(obj);

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(obj);
    }
}
