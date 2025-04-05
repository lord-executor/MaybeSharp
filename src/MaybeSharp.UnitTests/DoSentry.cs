using FluentAssertions;

namespace MaybeSharp.UnitTests;

class MapSentry<T>
    where T : class
{
    private bool JustCalled { get; set; }
    private bool NothingCalled { get; set; }
    private T? JustObject { get; set; }

    public T? JustActionDegenerate(T? obj)
    {
        JustObject = obj;
        JustCalled = true;
        return obj;
    }

    public IMaybe<T> JustAction(T? obj)
    {
        return Maybe.Of(JustActionDegenerate(obj));
    }

    public void VerifyJustCalled(T obj)
    {
        JustCalled.Should().BeTrue();
        JustObject.Should().BeSameAs(obj);
        NothingCalled.Should().BeFalse();
    }

    public void VerifyJustNotCalled()
    {
        JustCalled.Should().BeFalse();
        JustObject.Should().BeNull();
    }

    public T? NothingActionDegenerate()
    {
        NothingCalled = true;
        return null;
    }

    public IMaybe<T> NothingAction()
    {
        return Maybe.Of(NothingActionDegenerate());
    }

    public void VerifyNothingCalled()
    {
        VerifyJustNotCalled();
        NothingCalled.Should().BeTrue();
    }

    public void VerifyNothingNotCalled()
    {
        NothingCalled.Should().BeFalse();
    }
}
