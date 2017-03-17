using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.CompilerTest
{
    public class EnsureCompiledTest
    {
        [Fact]
        public void ReallyCompiled()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var compiler = new SdmapCompiler(ctx);
            compiler.AddSourceCode("sql v1{Hello World}");

            Assert.True(ctx.Emiters["v1"].Emiter == null);
            var ok = compiler.EnsureCompiled();
            Assert.True(ctx.Emiters["v1"].Emiter != null);
        }

        [Fact]
        public void SubsqlCanCompile()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var compiler = new SdmapCompiler(ctx);
            compiler.AddSourceCode("sql v1{#isNull<A, sql{test}}");

            var ok = compiler.EnsureCompiled();
            Assert.True(ok.IsSuccess);
            Assert.Equal(2, ctx.Emiters.Count);
        }
    }
}
