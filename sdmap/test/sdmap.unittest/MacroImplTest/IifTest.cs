using sdmap.Macros.Implements;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.MacroImplTest
{
    public class IifTest
    {
        [Fact]
        public void Smoke()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = true }, 
                new object[] { "A", "true", "false" });
            Assert.True(val.IsSuccess);
            Assert.Equal("true", val.Value);
        }

        [Fact]
        public void NullableOk()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?(true) },
                new object[] { "A", "true", "false" });
            Assert.True(val.IsSuccess);
            Assert.Equal("true", val.Value);
        }

        [Fact]
        public void EmptyNullShouldTreatFalsy()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?() },
                new object[] { "A", "true", "false" });
            Assert.True(val.IsSuccess);
            Assert.Equal("false", val.Value);
        }

        [Fact]
        public void NotExistPropShouldFail()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?() },
                new object[] { "B", "true", "false" });
            Assert.False(val.IsSuccess);
        }
    }
}
