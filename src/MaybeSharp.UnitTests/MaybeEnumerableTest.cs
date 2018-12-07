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
		private List<DemoType> _multiList = new List<DemoType> {
			new DemoType("Alpha", 0),
			new DemoType("Beta", 1),
			new DemoType("Gamma", 2),
			new DemoType("Delta", 3),
			new DemoType("Epsilon", 4),
			new DemoType("Zeta", 5),
		};

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
			result.Bind(d => d.Name).Extract().Should().Be("Alpha");
		}

		[Test]
		public void MaybeFirst__WithPredicate__ReturnsFirstFiltered()
		{
			var result = _multiList.MaybeFirst(x => x.Index > 2);

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Delta");
		}

		[Test]
		public void MaybeLast__OfEmptyList__ReturnsNothing()
		{
			var result = _emptyList.MaybeLast();

			result.Should().NotBeNull();
			result.Equals(Maybe.Nothing<DemoType>()).Should().BeTrue();
		}

		[Test]
		public void MaybeLast__OfList__ReturnsJustLast()
		{
			var result = _multiList.MaybeLast();

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Zeta");
		}

		[Test]
		public void MaybeLast__WithPredicate__ReturnsLastFiltered()
		{
			var result = _multiList.MaybeLast(x => x.Index % 3 == 0);

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Delta");
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

		[Test]
		public void MaybeSingle__WithPredicate__ReturnsSingleFiltered()
		{
			var result = _multiList.MaybeSingle(x => x.Index == 2);

			result.Should().NotBeNull();
			result.Bind(d => d.Name).Extract().Should().Be("Gamma");
		}
	}
}
