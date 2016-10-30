using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.VisitorTest
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Parser.G4;
    using Parser.Visitor;
    using Xunit;
    using static Parser.G4.SdmapParser;

    public class SqlItemVisitorTest
    {
        [Fact]
        public void CanDetectNamespace()
        {
            var pt = GetParseTree("namespace ns{sql sql{}}");
            var visitor = SqlItemVisitor.CreateEmpty();
            var result = visitor.Visit(pt);

            Assert.Equal(1, visitor.Context.Emiters.Count);
            Assert.Equal("ns.sql", visitor.Context.Emiters.First().Key);
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

        private RootContext GetParseTree(string sourceCode)
        {
            var inputStream = new AntlrInputStream(sourceCode);
            var baseLexer = new SdmapLexer(inputStream);
            var baseTokenStream = new CommonTokenStream(baseLexer);
            var parser = new SdmapParser(baseTokenStream);
            return parser.root();
        }
    }
}
