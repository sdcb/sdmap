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
        public void HelloWorld()
        {
            var code = "sql v1{#if(A){HelloWorld}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("HelloWorld", result);
        }
    }
}
