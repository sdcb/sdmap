using sdmap.Macros.Implements;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.MacroImplTest
{
    public class IfEmptyTest
    {
        [Fact]
        public void Smoke()
        {
            var val = CommonMacros.IfNotEmpty(SdmapContext.CreateEmpty(), 
                "", 
                new { A = "Nice" }, 
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void EmptyStringIsEmpty()
        {
            var val = CommonMacros.IfNotEmpty(SdmapContext.CreateEmpty(),
                "",
                new { A = " " },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void EmptyStringIsNotEmptyWhenIfNull()
        {
            var val = CommonMacros.IfNotNull(SdmapContext.CreateEmpty(),
                "",
                new { A = " " },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }

        [Fact]
        public void NullIsEmpty()
        {
            var val = CommonMacros.IfNotEmpty(SdmapContext.CreateEmpty(),
                "",
                new { A = (int?)null },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal(string.Empty, val.Value);
        }

        [Fact]
        public void ValueIsNotEmpty()
        {
            var val = CommonMacros.IfNotEmpty(SdmapContext.CreateEmpty(),
                "",
                new { A = DateTime.Now },
                new object[] { "A", "Ok" });
            Assert.True(val.IsSuccess);
            Assert.Equal("Ok", val.Value);
        }
    }
}
