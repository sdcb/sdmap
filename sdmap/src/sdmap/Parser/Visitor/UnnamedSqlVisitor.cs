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
using sdmap.Compiler;
using sdmap.Macros;
using Antlr4.Runtime;

namespace sdmap.Parser.Visitor
{
    public class UnnamedSqlVisitor : CoreSqlVisitor
    {
        public UnnamedSqlVisitor(SdmapCompilerContext context)
            : base(context)
        {
        }

        public static UnnamedSqlVisitor Create(SdmapCompilerContext context)
        {
            return new UnnamedSqlVisitor(context);
        }

        public static Result<EmitFunction> Compile(ParserRuleContext parseTree, SdmapCompilerContext context)
        {
            var visitor = Create(context);
            return visitor.Visit(parseTree)
                .OnSuccess(() => visitor.Function);
        }

        public static UnnamedSqlVisitor CreateEmpty()
        {
            return new UnnamedSqlVisitor(SdmapCompilerContext.CreateEmpty());
        }
    }
}
