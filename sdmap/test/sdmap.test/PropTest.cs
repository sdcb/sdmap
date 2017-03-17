using sdmap.Parser.Visitor;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class PropTest
    {
        [Fact]
        public void CanShowString()
        {
            var code = "sql v1{#prop<Name>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { Name = "Hello World" });
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void CanShowDecimal()
        {
            var code = "sql v1{#prop<V>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { V = 3.14m });
            Assert.Equal("3.14", result);
        }

        [Fact]
        public void CanShowDouble()
        {
            var code = "sql v1{#prop<V>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { V = 3.14 });
            Assert.Equal("3.14", result);
        }

        [Fact]
        public void CanShowDate()
        {
            var code = "sql v1{#prop<V>}";
            var now = DateTime.Now;
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { V = now });
            Assert.Equal(now.ToString(), result);
        }

        [Fact]
        public void CanShowEmpty()
        {
            var code = "namespace v1{sql v1{#prop<V>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1.v1", new { V = (int?)null});
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void NullTest()
        {
            var code = "namespace v1{sql v1{#prop<V>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1.v1", null);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NoPropTest()
        {
            var code = "namespace v1{sql v1{#prop<V>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1.v1", new { });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void NestedPropTest()
        {
            var code = "sql v1{#prop<A.B>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.TryEmit("v1", new { A = new { B = 3 } });
            Assert.Equal("3", result.Value);
        }
    }
}
