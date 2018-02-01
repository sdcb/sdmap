using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace sdmap.test
{
    public class SqlTextTest
    {
        [Fact]
        public void SlashHashWillEmitSingleHash()
        {
            var code = @"sql v1{\#}";
            var result = Run(code, "v1");
            Assert.Equal("#", result);
        }

        [Fact]
        public void SlashCurlyBraceWillEmitSingleHash()
        {
            var code = @"sql v1{\}}";
            var result = Run(code, "v1");
            Assert.Equal("}", result);
        }

        [Fact]
        public void ErrorCurlyBraceWillStillWork()
        {
            var code = "sql v1{}} sql v2{test}";
            var result = Run(code, "v2");
            Assert.Equal("test", result);
        }

        public static string Run(string code, string sqlId, object obj = null)
        {
            var c = new SdmapCompiler();
            c.AddSourceCode(code);
            return c.Emit(sqlId, obj);
        }
    }
}
