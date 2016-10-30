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
    using static SdmapLexer;
    using System.Text.RegularExpressions;
    using Utils;

    public class SqlItemVisitor : SdmapParserBaseVisitor<Result>
    {
        public SdmapContext Context { get; }

        private SqlItemVisitor(SdmapContext context)
        {
            Context = context;
        }

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            var openNs = context.GetToken(OpenNamespace, 0);
            var ns = LexerUtil.GetOpenNSId(openNs.GetText());

            Context.NsStack.Push(ns);
            var result = base.VisitNamespace(context);
            Context.NsStack.Pop();
            return result;
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            var openSql = context.GetToken(OpenNamedSql, 0);
            var id = LexerUtil.GetOpenSqlId(openSql.GetText());

            return Context.TryAdd(id, SqlEmiter.Create(context));
        }

        public static SqlItemVisitor Create(SdmapContext context)
        {
            return new SqlItemVisitor(context);
        }

        public static SqlItemVisitor CreateEmpty()
        {
            return new SqlItemVisitor(SdmapContext.CreateEmpty());
        }
    }
}
