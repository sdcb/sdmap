using sdmap.Compiler;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class ValTest
    {
        [Fact]
        public void CanShowValue()
        {
            var code = "sql v1{#val<>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", "Hello World");
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void EmptyValueTest()
        {
            var code = "sql v1{#val<>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal(string.Empty, result);
        }
    }
}
