using sdmap.Parser.Visitor;
using System.Linq;
using Xunit;

namespace sdmap.unittest.VisitorTest
{
    public class SqlItemVisitorTest : VisitorTestBase
    {
        [Fact]
        public void CanDetectNamespace()
        {
            var pt = GetParseTree("namespace ns{sql v1{}}");
            var visitor = SqlItemVisitor.CreateEmpty();
            var result = visitor.Visit(pt);

            Assert.Equal(1, visitor.Context.Emiters.Count);
            Assert.Equal("ns.v1", visitor.Context.Emiters.First().Key);
        }

        public void CanDetect2Namespaces()
        {
            var pt = GetParseTree("namespace ns{sql sql{} sql sql2{}}");
            var visitor = SqlItemVisitor.CreateEmpty();
            var result = visitor.Visit(pt);

            Assert.Equal(2, visitor.Context.Emiters.Count);
            Assert.Equal("ns.sql", visitor.Context.Emiters.First().Key);
            Assert.Equal("ns.sql2", visitor.Context.Emiters.Last().Key);
        }
    }
}
