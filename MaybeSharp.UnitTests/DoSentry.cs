using FluentAssertions;

namespace MaybeSharp.UnitTests
{
	class DoSentry<T>
	{
		public bool JustCalled { get; private set; }
		public bool NothingCalled { get; private set; }
		public T JustObject { get; private set; }

		public void JustAction(T obj)
		{
			JustObject = obj;
			JustCalled = true;
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

		public void NothingAction()
		{
			NothingCalled = true;
		}

		public void VerifyNothingCalled()
		{
			VerifyJustNotCalled();
			NothingCalled.Should().BeTrue();
		}
	}
}
