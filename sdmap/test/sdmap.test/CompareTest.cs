using sdmap.Compiler;
using System.IO;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class CompareTest
    {
        [Fact]
        public void IsEqualOk()
        {
            var code = "sql v1{#isEqual<A, true, 'Yes'>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("Yes", result);
        }

        [Fact]
        public void IsEqualIntOk()
        {
            var code = "sql v1{#isEqual<A, 3, 'Yes'>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = 3 });
            Assert.Equal("Yes", result);
        }

        [Theory]
        [InlineData(FileAccess.Read, "'Read'", true)]
        [InlineData(FileAccess.Read, "'read'", true)]
        [InlineData(FileAccess.Read, "1", true)]
        [InlineData(FileAccess.Read, "2", false)]
        public void IsEqualEnum(FileAccess? enumVal, string literal, bool canEmit)
        {
            var code = $"sql v1{{#isEqual<A, {literal}, 'Yes'>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = enumVal });
            var expected = canEmit ? "Yes" : "";
            Assert.Equal(expected, result);
        }
    }
}
