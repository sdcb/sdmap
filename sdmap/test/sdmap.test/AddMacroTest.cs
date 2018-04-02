using sdmap.Functional;
using sdmap.Macros;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using sdmap.Macros.Implements;

namespace sdmap.IntegratedTest
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
        public void CanImplementDepInMacro()
        {
            var code = "sql v1{#def<B, 'Nice'>#isNotEmptyWithDeps<A, sql {ABCDEFG}, B>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            string id = "isNotEmptyWithDeps";
            rt.AddMacro(id, (context, ns, self, arguments) =>
            {
                if (self == null) return Result.Fail<string>($"Query requires not null in macro '{id}'."); ;

                var prop = self.GetType().GetProperty((string)arguments[0]);
                if (prop == null) return Result.Fail<string>($"Query requires property '{prop}' in macro '{id}'.");

                if (!RuntimeMacros.IsEmpty(RuntimeMacros.GetPropValue(self, (string)arguments[0])))
                {
                    foreach (var dep in arguments.Skip(2).OfType<string>())
                    {
                        context.Deps.Add(dep);
                    }
                    return ((EmitFunction)arguments[1])(context.Dig(self));
                }   
                return Result.Ok(string.Empty);
            });
            var result = rt.Emit("v1", new { A = new[] { 1, 2, 3 } });
            Assert.Equal("NiceABCDEFG", result);
        }

        [Fact]
        public void CanAddArgumentMacro()
        {
            var code = "sql v1{#hello<sql{#val<>}>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            rt.AddMacro("hello", new[] { SdmapTypes.StringOrSql }, (context, ns, self, arguments) =>
            {
                context.Fragments.Add("Hello ");
                context.Fragments.Add(((EmitFunction)arguments[0])(context.Dig(self)).Value);
                return Result.Ok("");
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
