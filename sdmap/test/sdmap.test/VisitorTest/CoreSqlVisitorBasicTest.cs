using sdmap.Parser.Visitor;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.VisitorTest
{
    public class CoreSqlVisitorBasicTest : VisitorTestBase
    {
        [Fact]
        public void HelloWorld()
        {
            var code = "sql sql{Hello World}";
            var parseTree = GetParseTree(code);
            var visitor = CoreSqlVisitor.CreateEmpty();
            visitor.Visit(parseTree);
            
            Assert.NotNull(visitor.Function);

            var output = visitor.Function(SdmapContext.CreateEmpty(), null);
            Assert.Equal("Hello World", output.Value);
        }

        [Fact]
        public void SqlInNamespaceTest()
        {
            var sql = "SELECT * FROM `client_WOReactive`;";
            var code = "sql sql{" + sql + "}";
            var visitor = CoreSqlVisitor.CreateEmpty();
            var parseTree = GetParseTree(code);
            visitor.Visit(parseTree);
            
            Assert.NotNull(visitor.Function);

            var output = visitor.Function(SdmapContext.CreateEmpty(), null);
            Assert.Equal(sql, output.Value);
        }

        [Fact]
        public void MultiLineTest()
        {
            var sql = 
                "SELECT                  \r\n" +
                "   *                    \r\n" +
                "FROM                    \r\n" +
                "   `client_WOReactive`; \r\n";
            var code = $"sql sql{{{sql}}}";
            var visitor = CoreSqlVisitor.CreateEmpty();
            var parseTree = GetParseTree(code);
            visitor.Visit(parseTree);

            Assert.NotNull(visitor.Function);

            var output = visitor.Function(SdmapContext.CreateEmpty(), null);
            Assert.Equal(sql, output.Value);
        }
    }
}
