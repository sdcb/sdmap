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
        public void DoubleHashWillEmitSingleHash()
        {
            var code = "sql v1{##}";
            var result = Run(code, "v1");
            Assert.Equal("#", result);
        }

        public static string Run(string code, string sqlId)
        {
            var c = new SdmapCompiler();
            c.AddSourceCode(code);
            return c.Emit(sqlId, null);
        }
    }
}
