using sdmap.Functional;
using sdmap.Macros.Implements;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.MacroImplTest
{
    public class IsEmptyTest
    {
        [Fact]
        public void Smoke()
        {
            var val = CallIsNotEmpty(new { A = "NotEmpty" }, "A", "Ok");
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void EmptyStringIsEmpty()
        {
            var val = CallIsNotEmpty(new { A = " " }, "A", "Ok");
            Assert.True(val.IsSuccess);
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void EmptyStringIsNotEmptyWhenIfNull()
        {
            var val = CallIsNotNull(new { A = " " }, "A", "Ok");
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void NullIsEmpty()
        {
            var val = CallIsNotEmpty(new { A = (string)null }, "A", "Ok");
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void ValueIsNotEmpty()
        {
            var val = CommonMacros.IsNotEmpty(SdmapCompilerContext.CreateEmpty(),
                "",
                new { A = DateTime.Now },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        private Result<string> CallIsEmpty(object self, string prop, string result)
        {
            return CommonMacros.IsEmpty(SdmapCompilerContext.CreateEmpty(), "", self, new[] { prop, result });
        }

        private Result<string> CallIsNotEmpty(object self, string prop, string result)
        {
            return CommonMacros.IsNotEmpty(SdmapCompilerContext.CreateEmpty(), "", self, new[] { prop, result });
        }

        private Result<string> CallIsNotNull(object self, string prop, string result)
        {
            return CommonMacros.IsNotNull(SdmapCompilerContext.CreateEmpty(), "", self, new[] { prop, result });
        }
    }
}
