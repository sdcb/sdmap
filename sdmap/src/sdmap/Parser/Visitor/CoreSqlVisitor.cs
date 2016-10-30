using sdmap.Functional;
using sdmap.Parser.Context;
using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Parser.Visitor
{
    using ContextType = SortedDictionary<string, SqlEmiter>;

    public class CoreSqlVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly ContextType _context;
        private readonly EmitFunction _function;

        private CoreSqlVisitor(ContextType context)
        {
            _context = context;
        }

        public static CoreSqlVisitor Create(ContextType context)
        {
            return new CoreSqlVisitor(context);
        }

        public static Result<EmitFunction> Compile(CoreSqlContext parseTree, ContextType context)
        {
            var visitor = Create(context);
            return visitor.Visit(parseTree)
                .OnSuccess(() => visitor._function);
        }
    }
}
