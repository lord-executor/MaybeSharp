using FluentAssertions;

namespace MaybeSharp.UnitTests
{
	class MapSentry<T>
		where T : class
	{
		public bool JustCalled { get; private set; }
		public bool NothingCalled { get; private set; }
		public T JustObject { get; private set; }

		public T JustActionDegenerate(T obj)
		{
			JustObject = obj;
			JustCalled = true;
			return obj;
		}

		public IMaybe<T> JustAction(T obj)
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

		public T NothingActionDegenerate()
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
}
