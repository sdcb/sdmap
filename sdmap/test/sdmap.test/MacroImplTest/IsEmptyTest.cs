using sdmap.Functional;
using sdmap.Macros.Implements;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.MacroImplTest
{
    public class IsEmptyTest
    {
        [Fact]
        public void Smoke()
        {
            var val = CallIsNotEmpty(new { A = "NotEmpty" }, "Ok");
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void EmptyStringIsEmpty()
        {
            var val = CallIsNotEmpty(new { A = " " }, "Ok");
            Assert.True(val.IsSuccess);
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void EmptyStringIsNotEmptyWhenIfNull()
        {
            var val = CommonMacros.IsNotNull(SdmapContext.CreateEmpty(),
                "",
                new { A = " " },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void NullIsEmpty()
        {
            var val = CommonMacros.IsNotEmpty(SdmapContext.CreateEmpty(),
                "",
                new { A = (int?)null },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void ValueIsNotEmpty()
        {
            var val = CommonMacros.IsNotEmpty(SdmapContext.CreateEmpty(),
                "",
                new { A = DateTime.Now },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        private Result<string> CallIsEmpty(object self,  string result)
        {
            return CommonMacros.IsEmpty(SdmapContext.CreateEmpty(), "", self, new[] { result });
        }

        private Result<string> CallIsNotEmpty(object self, string result)
        {
            return CommonMacros.IsNotEmpty(SdmapContext.CreateEmpty(), "", self, new[] { result });
        }
    }
}
