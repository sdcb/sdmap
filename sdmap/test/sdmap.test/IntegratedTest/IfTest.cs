using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class IfTest
    {
        [Fact]
        public void TrueWillEmit()
        {
            var code = "sql v1{#if(A){HelloWorld}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void FalseWontEmit()
        {
            var code = "sql v1{#if(A){HelloWorld}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = false });
            Assert.Equal("", result);
        }

        [Theory]
        [InlineData(null, "== null", true)]
        [InlineData(null, "!= null", false)]
        [InlineData("nn", "== null", false)]
        [InlineData("nn", "!= null", true)]
        public void EqualNullTest(string aValue, string nullOperator, bool willEmit)
        {
            var code = $"sql v1{{#if(A {nullOperator}){{HelloWorld}}}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = aValue });
            if (willEmit)
            {
                Assert.Equal("HelloWorld", result);
            }
            else
            {
                Assert.Equal("", result);
            }
        }
    }
}
