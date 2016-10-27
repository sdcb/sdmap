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
    using ContextType = SortedDictionary<string, SqlEmiter>;

    public class SqlItemVisitorTest
    {
        [Fact]
        public void CanDetectNamespace()
        {
            var pt = GetParseTree("namespace ns{sql sql{}}");
            var ctx = SqlItemVisitorContext.Create();
            var result = ctx.Visitor.Visit(pt);

            Assert.Equal(1, ctx.Context.Count);
            Assert.Equal("ns.sql", ctx.Context.First().Key);
        }

        public RootContext GetParseTree(string sourceCode)
        {
            var inputStream = new AntlrInputStream(sourceCode);
            var baseLexer = new SdmapLexer(inputStream);
            var baseTokenStream = new CommonTokenStream(baseLexer);
            var parser = new SdmapParser(baseTokenStream);
            return parser.root();
        }
    }

    public class SqlItemVisitorContext
    {
        public Stack<string> NsStack { get; set; } = new Stack<string>();

        public ContextType Context { get; set; } = new ContextType();

        public SqlItemVisitor Visitor { get; set; }

        public static SqlItemVisitorContext Create()
        {
            var ctx = new SqlItemVisitorContext();
            ctx.Visitor = SqlItemVisitor.Create(ctx.Context, ctx.NsStack);
            return ctx;
        }
    }
}
