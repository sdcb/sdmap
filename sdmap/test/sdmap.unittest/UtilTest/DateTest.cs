using sdmap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.UtilTest
{
    public class DateTest
    {
        [Fact]
        public void SlashOk()
        {
            var date = "2016/1/1";
            var result = DateUtil.Parse(date);

            Assert.Equal(true, result.IsSuccess);
            Assert.Equal(new DateTime(2016, 1, 1), result.Value);
        }

        [Fact]
        public void DashOk()
        {
            var date = "2016-1-1";
            var result = DateUtil.Parse(date);

            Assert.Equal(true, result.IsSuccess);
            Assert.Equal(new DateTime(2016, 1, 1), result.Value);
        }

        [Fact]
        public void OutOfRangeFail()
        {
            var date = "2016-1-100";
            var result = DateUtil.Parse(date);

            Assert.Equal(false, result.IsSuccess);
        }

        [Fact]
        public void ZeroFail()
        {
            var date = "2016-1-0";
            var result = DateUtil.Parse(date);

            Assert.Equal(false, result.IsSuccess);
        }
    }
}
