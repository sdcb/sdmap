using sdmap.Parser.Visitor;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class ValTest
    {
        [Fact]
        public void CanShowValue()
        {
            var code = "sql v1{#val<>}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", "Hello World");
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void EmptyValueTest()
        {
            var code = "sql v1{#val<>}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal(string.Empty, result);
        }
    }
}
