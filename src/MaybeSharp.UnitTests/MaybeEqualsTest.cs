using FluentAssertions;
using NUnit.Framework;

namespace MaybeSharp.UnitTests;

[TestFixture]
public class MaybeEqualsTest
{
    public delegate bool Comparison<T>(IMaybe<T> a, IMaybe<T>? b) where T : class;

    private static readonly Comparison<DemoType>[] Comparisons =
    [
        (a, b) => a.Equals(b),
        (a, b) => a.Equals((object?)b)
    ];

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__JustWithSameInstance__IsTrue(Comparison<DemoType> compare)
    {
        var obj = new DemoType();
        var maybe = Maybe.Of(obj);

        var result = compare(maybe, maybe);

        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__JustWithSameObject__IsTrue(Comparison<DemoType> compare)
    {
        var obj = new DemoType();

        var result = compare(Maybe.Of(obj), Maybe.Of(obj));

        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__JustWithNull__IsFalse(Comparison<DemoType> compare)
    {
        var obj = new DemoType();

        var result = compare(Maybe.Of(obj), null);

        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__JustWithNothing__IsFalse(Comparison<DemoType> compare)
    {
        var obj = new DemoType();

        var result = compare(Maybe.Of(obj), Maybe.Nothing<DemoType>());

        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__NothingWithNothing__IsTrue(Comparison<DemoType> compare)
    {
        var result = compare(Maybe.Nothing<DemoType>(), Maybe.Nothing<DemoType>());

        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(Comparisons))]
    public void Equals__NothingWithNull__IsFalse(Comparison<DemoType> compare)
    {
        var result = compare(Maybe.Nothing<DemoType>(), null);

        result.Should().BeFalse();
    }

    [Test]
    public void Equals__NothingWithOtherNothing__IsFalse()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        // Yes, this comparison is stupid, but that's the point. It should still be false as the two are
        // different nothings
        var result = Maybe.Nothing<DemoType>().Equals(Maybe.Nothing<object>());

        result.Should().BeFalse();
    }
}
