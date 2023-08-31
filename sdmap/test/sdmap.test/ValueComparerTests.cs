using System;
using System.Collections.Generic;
using sdmap.Macros.Implements;
using Xunit;

namespace sdmap.test;

using static ComparisonResult;

public class ValueComparerTests
{
    [Theory]
    [MemberData(nameof(Compare_Cases_Number))]
    [MemberData(nameof(Compare_Cases_Enum))]
    [MemberData(nameof(Compare_Cases_Guid))]
    [MemberData(nameof(Compare_Cases_Timespan))]
    [MemberData(nameof(Compare_Cases_DateTime))]
    public void Compare(object left, object right, ComparisonResult expected)
    {
        var actual = ValueComparer.Compare(left, right);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> Compare_Cases_Number()
    {
        yield return new object[] { 0, 0, AreEqual };
        yield return new object[] { 0, 0L, AreEqual };
        yield return new object[] { (short) 1, 1L, AreEqual };
        yield return new object[] { (byte) 1, (ulong) 1, AreEqual };

        yield return new object[] { 1D, 1F, AreEqual };
        yield return new object[] { 1M, 1M, AreEqual };
        yield return new object[] { 1M, 1D, AreEqual };
        yield return new object[] { 1M, 0D, LeftIsGreater };

        yield return new object[] { 1M, "1", AreEqual };
    }

    public static IEnumerable<object[]> Compare_Cases_Enum()
    {
        yield return new object[] { FirstEnum.One, FirstEnum.One, AreEqual };
        yield return new object[] { FirstEnum.One, FirstEnum.Two, LeftIsLess };
        yield return new object[] { FirstEnum.One, SecondEnum.One, Incomparable };
        yield return new object[] { FirstEnum.One, SecondEnum.Two, Incomparable };

        yield return new object[] { FirstEnum.One, 1, AreEqual };
        yield return new object[] { FirstEnum.One, "1", AreEqual };
        yield return new object[] { FirstEnum.One, "One", AreEqual };
        yield return new object[] { FirstEnum.One, 1.0M, Incomparable };
    }

    public static IEnumerable<object[]> Compare_Cases_Guid()
    {
        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            AreEqual
        };

        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            "01d1692d-2221-47cd-892b-4a6552c6a9cc",
            AreEqual
        };

        yield return new object[]
        {
            new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("11111111-0000-0000-0000-000000000000"),
            LeftIsLess
        };

        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            DateTime.Now,
            Incomparable
        };
    }
    
    public static IEnumerable<object[]> Compare_Cases_Timespan()
    {
        yield return new object[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), AreEqual };
        yield return new object[] { TimeSpan.FromSeconds(1), "00:00:01", AreEqual };
        yield return new object[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0), LeftIsGreater };
        yield return new object[] { TimeSpan.FromSeconds(1), "ten seconds", Incomparable };
        yield return new object[] { TimeSpan.FromSeconds(1), DateTime.Now, Incomparable };
    }

    public static IEnumerable<object[]> Compare_Cases_DateTime()
    {
        yield return new object[] { new DateTime(2023, 01, 01), new DateTime(2023, 01, 01), AreEqual };
        yield return new object[] { new DateTime(2023, 01, 01), new DateTime(1900, 01, 01), LeftIsGreater };
        yield return new object[] { new DateTime(2023, 01, 01), "2023-01-01", AreEqual };
        yield return new object[] { DateTime.Now, "today", Incomparable };
    }

    private enum FirstEnum { One = 1, Two = 2 }

    private enum SecondEnum { One = 1, Two = 2 }
}