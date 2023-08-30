using System;
using System.Collections.Generic;
using sdmap.Macros.Implements;
using Xunit;

namespace sdmap.test;

public class ValueComparerTests
{
    [Theory]
    [MemberData(nameof(IsEqual_Cases_Number))]
    [MemberData(nameof(IsEqual_Cases_Enum))]
    [MemberData(nameof(IsEqual_Cases_Guid))]
    [MemberData(nameof(IsEqual_Cases_Timespan))]
    [MemberData(nameof(IsEqual_Cases_DateTime))]
    public void IsEqual(object left, object right, bool areEqual)
    {
        var actual = ValueComparer.IsEqual(left, right);
        Assert.Equal(areEqual, actual);
    }

    public static IEnumerable<object[]> IsEqual_Cases_Number()
    {
        yield return new object[] { 0, 0, true };
        yield return new object[] { 0, 0L, true };
        yield return new object[] { (short) 1, 1L, true };
        yield return new object[] { (byte) 1, (ulong) 1, true };

        yield return new object[] { 1D, 1F, true };
        yield return new object[] { 1M, 1M, true };
        yield return new object[] { 1M, 1D, true };
        yield return new object[] { 1M, 0D, false };

        yield return new object[] { 1M, "1", true };
    }

    public static IEnumerable<object[]> IsEqual_Cases_Enum()
    {
        yield return new object[] { FirstEnum.One, FirstEnum.One, true };
        yield return new object[] { FirstEnum.One, FirstEnum.Two, false };
        yield return new object[] { FirstEnum.One, SecondEnum.One, false };
        yield return new object[] { FirstEnum.One, SecondEnum.Two, false };

        yield return new object[] { FirstEnum.One, 1, true };
        yield return new object[] { FirstEnum.One, "1", true };
        yield return new object[] { FirstEnum.One, "One", true };
        yield return new object[] { FirstEnum.One, 1.0M, false };
    }

    public static IEnumerable<object[]> IsEqual_Cases_Guid()
    {
        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            true
        };

        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            "01d1692d-2221-47cd-892b-4a6552c6a9cc",
            true
        };

        yield return new object[] { Guid.NewGuid(), Guid.NewGuid(), false };

        yield return new object[]
        {
            new Guid("01d1692d-2221-47cd-892b-4a6552c6a9cc"),
            DateTime.Now,
            false
        };
    }
    
    public static IEnumerable<object[]> IsEqual_Cases_Timespan()
    {
        yield return new object[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), true };
        yield return new object[] { TimeSpan.FromSeconds(1), "00:00:01", true };
        yield return new object[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0), false };
        yield return new object[] { TimeSpan.FromSeconds(1), "10", false };
        yield return new object[] { TimeSpan.FromSeconds(1), DateTime.Now, false };
    }

    public static IEnumerable<object[]> IsEqual_Cases_DateTime()
    {
        yield return new object[] { new DateTime(2023, 01, 01), new DateTime(2023, 01, 01), true };
        yield return new object[] { new DateTime(2023, 01, 01), new DateTime(1900, 01, 01), false };
        yield return new object[] { new DateTime(2023, 01, 01), "2023-01-01", true };
        yield return new object[] { DateTime.Now, TimeSpan.FromSeconds(0), false };
    }

    private enum FirstEnum { One = 1, Two = 2 }

    private enum SecondEnum { One = 1, Two = 2 }
}