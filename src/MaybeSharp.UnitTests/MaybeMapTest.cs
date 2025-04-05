using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeMapTest
{
    [Test]
    public void MaybeOfNullValue__Map__CallsNothingAction()
    {
        var maybe = Maybe.Of<DemoType>(null);
        var sentry = new MapSentry<DemoType>();

        maybe.Map(sentry.JustAction, sentry.NothingAction);

        sentry.VerifyNothingCalled();
        sentry.VerifyJustNotCalled();
    }

    [Test]
    public void MaybeOfObject__Map__CallsJustAction()
    {
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);
        var sentry = new MapSentry<DemoType>();

        maybe.Map(sentry.JustAction, sentry.NothingAction);

        sentry.VerifyJustCalled(obj);
        sentry.VerifyNothingNotCalled();
    }

    [Test]
    public void MaybeOfNullValue__MapDegenerate__CallsNothingAction()
    {
        var maybe = Maybe.Of<DemoType>(null);
        var sentry = new MapSentry<DemoType>();

        maybe.Map(sentry.JustActionDegenerate, sentry.NothingActionDegenerate);

        sentry.VerifyNothingCalled();
        sentry.VerifyJustNotCalled();
    }

    [Test]
    public void MaybeOfObject__MapDegenerate__CallsJustAction()
    {
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);
        var sentry = new MapSentry<DemoType>();

        maybe.Map(sentry.JustActionDegenerate, sentry.NothingActionDegenerate);

        sentry.VerifyJustCalled(obj);
        sentry.VerifyNothingNotCalled();
    }
}
