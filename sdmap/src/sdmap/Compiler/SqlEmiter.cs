using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Compiler
{
    public static class SqlEmiter
    {
        public static SqlEmiterBase Create(NamedSqlContext parseTree, string ns)
        {
            return new SqlEmiterBase(parseTree, ns, 
                (ctx, pt) => Compile(ctx, (NamedSqlContext)pt));
        }

        private static Result<EmitFunction> Compile(
            SdmapCompilerContext context, 
            NamedSqlContext parseTree)
        {
            return NamedSqlVisitor.Compile(parseTree, context);
        }
    }
}
