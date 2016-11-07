using sdmap.Parser.Visitor;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class IifTest
    {
        [Theory]
        [InlineData(true, "Yes#")]
        [InlineData(false, "No!<>")]
        public void YesNo(bool input, string expected)
        {
            var code = "sql v1{#iif<A, 'Yes#', 'No!<>'}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = input });
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InsuficientArgumentShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#'}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NumberTypeShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 3.14}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void DateTypeShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 2016-1-1}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NotExistPropShouldFail()
        {
            var code = "sql v1{#iif<A, 'Yes#', 'hello'}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { B = true });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CanNestUnnamedSql()
        {
            var code = "sql v1{#iif<A, sql{Hello}, sql{World}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = true });
            Assert.True(result.IsSuccess);
            Assert.Equal("Hello", result.Value);
        }
    }
}
