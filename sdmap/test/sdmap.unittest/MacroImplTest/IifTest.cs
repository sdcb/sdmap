using sdmap.Macros.Implements;
using sdmap.Compiler;
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
                ["A", "true", "false"]);
            Assert.True(val.IsSuccess);
            Assert.Equal("true", val.Value);
        }

        [Fact]
        public void NullableOk()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?(true) },
                ["A", "true", "false"]);
            Assert.True(val.IsSuccess);
            Assert.Equal("true", val.Value);
        }

        [Fact]
        public void EmptyNullShouldTreatFalsy()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?() },
                ["A", "true", "false"]);
            Assert.True(val.IsSuccess);
            Assert.Equal("false", val.Value);
        }

        [Fact]
        public void NotExistPropShouldFail()
        {
            var val = DynamicRuntimeMacros.Iif(OneCallContext.CreateEmpty(),
                "",
                new { A = new bool?() },
                ["B", "true", "false"]);
            Assert.False(val.IsSuccess);
        }
    }
}
