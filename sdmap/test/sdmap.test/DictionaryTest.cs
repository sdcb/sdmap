using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test
{
    public class DictionaryTest
    {
        [Fact]
        public void MacroTest()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#prop<A>}");
            var result = rt.TryEmit("v1", new Dictionary<string, object>
            {
                ["A"] = "A"
            });
            Assert.True(result.IsSuccess);
            Assert.Equal("A", result.Value);
        }

        [Fact]
        public void IfTest()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#if(A){A}}");
            var result = rt.TryEmit("v1", new Dictionary<string, object>
            {
                ["A"] = true
            });
            Assert.True(result.IsSuccess);
            Assert.Equal("A", result.Value);
        }
    }
}
