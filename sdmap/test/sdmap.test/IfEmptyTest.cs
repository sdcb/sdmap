using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class IfEmptyTest
    {
        [Fact]
        public void isNotEmpty()
        {
            var code = "sql v1{#isNotEmpty<A, sql{@A}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = "NotEmpty" });
            Assert.Equal("@A", result);
        }

        [Fact]
        public void IfNotNull()
        {
            var code = "sql v1{#isNotEmpty<A, sql{@A}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = " " });
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void PropInSubSql()
        {
            var code = "sql v1{#isNotEmpty<A, sql{#prop<A>}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = "NotEmpty" });
            Assert.Equal("NotEmpty", result);
        }
    }
}
