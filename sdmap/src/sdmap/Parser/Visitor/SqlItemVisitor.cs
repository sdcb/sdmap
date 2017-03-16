using Antlr4.Runtime.Misc;
using sdmap.Functional;
using sdmap.Parser.G4;
using sdmap.Utils;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapLexer;

namespace sdmap.Parser.Visitor
{
    public class SqlItemVisitor : SdmapParserBaseVisitor<Result>
    {
        public SdmapCompilerContext Context { get; }

        private SqlItemVisitor(SdmapCompilerContext context)
        {
            Context = context;
        }

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            var ns = context.nsSyntax().GetText();

            Context.NsStack.Push(ns);
            var result = base.VisitNamespace(context);
            Context.NsStack.Pop();
            return result;
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            var id = context.GetToken(SYNTAX, 0).GetText();

            return Context.TryAdd(id, SqlEmiter.Create(context, Context.CurrentNs));
        }

        public static SqlItemVisitor Create(SdmapCompilerContext context)
        {
            return new SqlItemVisitor(context);
        }

        public static SqlItemVisitor CreateEmpty()
        {
            return new SqlItemVisitor(SdmapCompilerContext.CreateEmpty());
        }
    }
}
