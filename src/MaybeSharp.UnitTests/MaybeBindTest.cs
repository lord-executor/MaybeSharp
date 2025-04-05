using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeBindTest
{
    [Test]
    public void MaybeOfNullValue__Bind__DoesNotCallBinding()
    {
        var sentry = false;
        var maybe = Maybe.Of<DemoType>(null);

        var result = maybe.Bind(d => {
            sentry = true;
            return Maybe.Of(d);
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
            return Maybe.Of(d);
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

    [Test]
    public void MaybeOfNullValue__DegenerateBind__DoesNotCallBinding()
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
    public void MaybeOfObject__DegenerateBind__CallsBinding()
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
    public void Bind__OnTargetProperty__JustResult()
    {
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);

        var result = maybe.Bind(x => x.Name);

        result.Should().NotBeNull();
        result.Extract().Should().BeSameAs(obj.Name);
    }

    [Test]
    public void Bind__OnTargetPropertyWithNullValue__NothingResult()
    {
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);

        var result = maybe.Bind(x => x.Optional);

        result.Should().NotBeNull();
        result.Extract().Should().BeNull();
    }
}
