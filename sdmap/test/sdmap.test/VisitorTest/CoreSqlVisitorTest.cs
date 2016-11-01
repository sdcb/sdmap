using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.VisitorTest
{
    public class CoreSqlVisitorTest : VisitorTestBase
    {
        [Fact]
        public void HelloWorld()
        {
            var code = "sql sql{Hello World}";
            var parseTree = GetParseTree(code);
            var visitor = CoreSqlVisitor.CreateEmpty();
            var result = visitor.Visit(parseTree);

            Assert.True(result.IsSuccess);
            Assert.NotNull(visitor.Function);

            var output = visitor.Function(null);
            Assert.Equal("Hello World", output);
        }
    }
}
