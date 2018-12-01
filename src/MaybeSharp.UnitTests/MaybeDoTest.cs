﻿using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeDoTest
	{
		[Test]
		public void Do__OnNothing__DoesNotCallJustAction()
		{
			var maybe = Maybe.Of<DemoType>(null);
			var sentry = false;

			maybe.Do(v => { sentry = true; });

			sentry.Should().BeFalse();
		}

		[Test]
		public void Do__OnNothing__CallsNothingAction()
		{
			var maybe = Maybe.Of<DemoType>(null);
			var sentry = false;

			maybe.Do(v => { }, () => { sentry = true; });

			sentry.Should().BeTrue();
		}

		[Test]
		public void Do__OnObject__CallsJustAction()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);
			var sentry = false;

			maybe.Do(v => { sentry = true; });

			sentry.Should().BeTrue();
		}

		[Test]
		public void Do__OnObject__DoesNotCallNothingAction()
		{
			var obj = new DemoType();
			var maybe = Maybe.Of(obj);
			var sentry = false;

			maybe.Do(v => { }, () => { sentry = true; });

			sentry.Should().BeFalse();
		}
	}
}
