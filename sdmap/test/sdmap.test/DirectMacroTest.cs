using sdmap.Compiler;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class DirectMacroTest
    {
        [Fact]
        public void Simple()
        {
            var code = "sql v1{#isEqual<A, true, #prop<B>>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true, B = "Yes" });
            Assert.Equal("Yes", result);
        }

        [Fact]
        public void PropNotExists()
        {
            var code = "sql v1{#isEqual<A, true, #prop<B>>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
            Assert.Equal("Query requires property 'B' in macro 'Prop'.", result.Error);
        }
    }
}
