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
    public static class NamedSqlVisitor
    {
        public static Result<EmitFunction> Compile(
            NamedSqlContext parseTree, 
            SdmapCompilerContext context)
        {
            var id = parseTree.GetToken(SYNTAX, 0).GetText();
            var fullName = context.GetFullNameInCurrentNs(id);

            var core = new CoreSqlVisitor(context);
            return core.Process(parseTree.coreSql(), fullName)
                .OnSuccess(() => core.Function);
        }

        public static Result<EmitFunction> CompileNoContext(
            NamedSqlContext parseTree)
        {
            return Compile(parseTree, SdmapCompilerContext.CreateEmpty());
        }
    }
}
