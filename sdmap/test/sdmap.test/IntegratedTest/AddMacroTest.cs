using sdmap.Functional;
using sdmap.Macros;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class AddMacroTest
    {
        [Fact]
        public void CanAddMacro()
        {
            var code = "sql v1{#hello<>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            rt.AddMacro("hello", new SdmapTypes[0], (context, ns, self, arguments) =>
            {
                return Result.Ok("Hello World");
            });
            var result = rt.Emit("v1", null);
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void CanAddArgumentMacro()
        {
            var code = "sql v1{#hello<sql{#val<>}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            rt.AddMacro("hello", new[] { SdmapTypes.StringOrSql }, (context, ns, self, arguments) =>
            {
                return Result.Ok($"Hello " + 
                    MacroUtil.EvalToString(arguments[0], context, self).Value);
            });
            var result = rt.Emit("v1", "World");
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void AddArgumentWillDoRuntimeCheck()
        {
            var code = "sql v1{#hello<3>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            rt.AddMacro("hello", new SdmapTypes[0], (context, ns, self, arguments) =>
            {
                return Result.Ok("Hello World");
            });
            var result = rt.TryEmit("v1", null);
            Assert.False(result.IsSuccess);
        }
    }
}
