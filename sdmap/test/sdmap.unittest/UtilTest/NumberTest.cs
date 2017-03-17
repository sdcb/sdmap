using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using static sdmap.Utils.NumberUtil;

namespace sdmap.unittest.UtilTest
{
    public class NumberTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("　")]
        public void EmptyWillFail(string input)
        {
            var escaped = Parse(input);
            Assert.True(escaped.IsFailure);
        }

        [Theory]
        [InlineData("3", 3)]
        [InlineData("9876", 9876)]
        [InlineData("3.14", 3.14)]
        [InlineData("-5.5", -5.5)]
        [InlineData("-3e-2", -3e-2)]
        public void NormalConvertCanSuccess(string input, double expected)
        {
            var actual = Parse(input);
            Assert.True(actual.IsSuccess);
            Assert.Equal(expected, actual.Value);
        }
    }
}
