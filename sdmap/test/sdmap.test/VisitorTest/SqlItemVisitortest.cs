using Antlr4.Runtime;
using sdmap.Parser.Context;
using sdmap.Parser.G4;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.test.VisitorTest
{
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
            var lexer = new SdmapLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SdmapParser(tokenStream);
            return parser.root();
        }
    }
}
