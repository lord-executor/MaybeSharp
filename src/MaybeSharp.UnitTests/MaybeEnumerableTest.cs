using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace MaybeSharp.UnitTests
{
	[TestFixture]
	public class MaybeEnumerableTest
	{
		private List<DemoType> _emptyList = new List<DemoType>();
		private List<DemoType> _singleList = new List<DemoType> { new DemoType("Single") };
		private List<DemoType> _multiList = new List<DemoType> { new DemoType("First"), new DemoType("Second"), new DemoType("Last") };

		[Test]
		public void MaybeFirst__OfEmptyList__ReturnsNothing()
		{
			var result = _emptyList.MaybeFirst();

			result.Should().NotBeNull();
			result.Should().Be(Maybe.Nothing<DemoType>());
		}

		[Test]
		public void MaybeFirst__OfList__ReturnsJustFirst()
		{
			var result = _multiList.MaybeFirst();

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("First");
		}

		[Test]
		public void MaybeLast__OfEmptyList__ReturnsNothing()
		{
			var result = _emptyList.MaybeLast();

			result.Should().NotBeNull();
			result.Equals(Maybe.Nothing<DemoType>()).Should().BeTrue();
		}

		[Test]
		public void MaybeLast__OfList__ReturnsJustFirst()
		{
			var result = _multiList.MaybeLast();

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Last");
		}

		[Test]
		public void MaybeSingle__OfEmptyList__ReturnsNothing()
		{
			var result = _emptyList.MaybeSingle();

			result.Should().NotBeNull();
			result.Equals(Maybe.Nothing<DemoType>()).Should().BeTrue();
		}

		[Test]
		public void MaybeSingle__OfSingleItem__ReturnsJustItem()
		{
			var result = _singleList.MaybeSingle();

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Single");
		}
	}
}
