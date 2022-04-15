using sdmap.Functional;
using sdmap.Compiler;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class IifTest
    {
        [Theory]
        [InlineData(true, "Yes#")]
        [InlineData(false, "No!<>")]
        public void YesNo(bool input, string expected)
        {
            var code = "sql v1{#iif<A, 'Yes#', 'No!<>'}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = input });
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InsuficientArgumentShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#'}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NumberTypeShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 3.14}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void DateTypeShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 2016-1-1}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NotExistPropShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 'hello'}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { B = true });
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No!<>")]
        public void CanNestUnnamedSql(bool input, string expected)
        {
            var code = "sql v1{#iif<A, sql{Yes}, sql{No!<>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = input });
            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void CanRunInnerMacro()
        {
            var code = "sql v1{#iif<A, sql{#prop<A>}, sql{}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.True(result.IsSuccess);
            Assert.Equal(true.ToString(), result.Value);
        }

        [Fact]
        public void OnlyRunOnce()
        {
            var code = "sql v1{#iif<A, sql{#record<>}, sql{#record<>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var times = 0;
            rt.AddMacro("record", null, (ctx, ns, self, args) =>
            {
                ++times;
                return Result.Ok(string.Empty);
            });
            var result = rt.TryEmit("v1", new { A = true });

            Assert.True(result.IsSuccess);
            Assert.Equal(1, times);
        }
    }
}
