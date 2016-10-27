using sdmap.Functional;
using sdmap.Parser.Context;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Parser.Visitor
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using ContextType = SortedDictionary<string, SqlEmiter>;
    using static SdmapLexer;
    using System.Text.RegularExpressions;
    using Utils;

    public class SqlItemVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly ContextType _context;

        private readonly Stack<string> _nsStack;

        public string CurrentNamespace => _nsStack.Peek();

        private SqlItemVisitor(ContextType context, Stack<string> nsStack)
        {
            _context = context;
            _nsStack = nsStack;
        }

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            var openNs = context.GetToken(OpenNamespace, 0);
            var ns = LexerUtil.GetOpenNSId(openNs.GetText());

            _nsStack.Push(ns);
            var result = base.VisitNamespace(context);
            _nsStack.Pop();
            return result;
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            var openSql = context.GetToken(OpenNamedSql, 0);
            var id = LexerUtil.GetOpenSqlId(openSql.GetText());

            var fullName = CurrentNamespace + "." + id;
            var coreSql = context.GetChild<SdmapParser.CoreSqlContext>(0);
            _context.Add(fullName, SqlEmiter.Create(coreSql));

            return base.VisitNamedSql(context);
        }

        public static SqlItemVisitor CreateEmpty()
        {
            return CreateByContext(new ContextType());
        }

        public static SqlItemVisitor CreateByContext(ContextType context)
        {
            return Create(context, new Stack<string>());
        }

        public static SqlItemVisitor Create(ContextType context, Stack<string> nsStack)
        {
            return new SqlItemVisitor(context, nsStack);
        }
    }
}
