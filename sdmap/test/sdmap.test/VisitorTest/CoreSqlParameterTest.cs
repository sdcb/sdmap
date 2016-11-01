using sdmap.Parser.Visitor;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.VisitorTest
{
    public class CoreSqlParameterTest : VisitorTestBase
    {
        [Fact]
        public void Include()
        {
            var code = "sql v1{#include<v2>} sql v2{Life is good}";
            var rt = new SdmapManager();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal("Life is good", result);
        }
    }
}
