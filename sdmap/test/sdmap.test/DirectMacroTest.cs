using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
