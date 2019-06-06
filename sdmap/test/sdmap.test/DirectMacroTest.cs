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

        [Fact]
        public void IifPropTest()
        {
            var code = @"sql v1{#iif<A, #prop<IsTrue>, #prop<IsFalse>>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true, IsTrue = "A=True", IsFalse = "A=False" });
            Assert.Equal("A=True", result);
        }

        [Fact]
        public void IncludeInThisSenarioNotWorking()
        {
            var code = @"sql v1{#iif<A, #include<v2>, #include<v3>>}
                         sql v2{A=true}
                         sql v3{A=false}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("A=trueA=false", result);
        }

        [Fact]
        public void CorrectIncludePattern()
        {
            var code = @"sql v1{#iif<A, sql{#include<v2>}, sql{#include<v3>}>}
                         sql v2{A=true}
                         sql v3{A=false}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("A=true", result);
        }
    }
}
