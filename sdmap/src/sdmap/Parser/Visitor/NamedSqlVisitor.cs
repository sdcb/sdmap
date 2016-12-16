using sdmap.Functional;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;
using Antlr4.Runtime.Misc;
using sdmap.Utils;
using System.Text;
using sdmap.Runtime;
using sdmap.Macros;
using Antlr4.Runtime;

namespace sdmap.Parser.Visitor
{
    public class NamedSqlVisitor : CoreSqlVisitor
    {
        public NamedSqlVisitor(SdmapContext context)
            : base(context)
        {
        }

        protected override string GetFunctionName(ParserRuleContext parseRule)
        {
            var context = (NamedSqlContext)parseRule;
            var openSql = context.GetToken(OpenNamedSql, 0);
            var id = LexerUtil.GetOpenSqlId(openSql.GetText());
            var fullName = _context.GetFullNameInCurrentNs(id);
            return fullName;
        }

        public static NamedSqlVisitor Create(SdmapContext context)
        {
            return new NamedSqlVisitor(context);
        }

        public static Result<EmitFunction> Compile(NamedSqlContext parseTree, SdmapContext context)
        {
            var visitor = Create(context);
            return visitor.Visit(parseTree)
                .OnSuccess(() => visitor.Function);
        }

        public static NamedSqlVisitor CreateEmpty()
        {
            return new NamedSqlVisitor(SdmapContext.CreateEmpty());
        }
    }
}
