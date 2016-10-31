using sdmap.Functional;
using sdmap.Parser.Context;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;
using Antlr4.Runtime.Misc;
using sdmap.Parser.Utils;

namespace sdmap.Parser.Visitor
{
    public class CoreSqlVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly SdmapContext _context;
        private EmitFunction _function;
        private readonly ILGenerator _il;

        private CoreSqlVisitor(SdmapContext context)
        {
            _context = context;
        }

        public override Result VisitNamedSql([NotNull] NamedSqlContext context)
        {
            var openSql = context.GetToken(OpenNamedSql, 0);
            var id = LexerUtil.GetOpenSqlId(openSql.GetText());
            var fullName = _context.GetFullName(id);

            var method = new DynamicMethod(fullName, typeof(string), new[] { typeof(object) });
            _il = method.GetILGenerator();
            _function = (EmitFunction)method.CreateDelegate(typeof(EmitFunction));
            return Result.Ok();
        }

        public static CoreSqlVisitor Create(SdmapContext context)
        {
            return new CoreSqlVisitor(context);
        }

        public static Result<EmitFunction> Compile(NamedSqlContext parseTree, SdmapContext context)
        {
            var visitor = Create(context);
            return visitor.Visit(parseTree)
                .OnSuccess(() => visitor._function);
        }
    }
}
