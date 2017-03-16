using Antlr4.Runtime;
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
    public class UnnamedSqlEmiter : SqlEmiterBase
    {
        public UnnamedSqlEmiter(ParserRuleContext parseTree, string ns)
            : base(parseTree, ns)
        {
        }

        public static UnnamedSqlEmiter Create(ParserRuleContext parseTree, string ns)
        {
            return new UnnamedSqlEmiter(parseTree, ns);
        }

        protected override Result<EmitFunction> Compile(SdmapCompilerContext context)
        {
            if (_parseTree is UnnamedSqlContext)
            {
                return CoreSqlVisitor.CompileCore(
                    (_parseTree as UnnamedSqlContext).coreSql(),
                    context);
            }
            else if (_parseTree is CoreSqlContext)
            {
                return CoreSqlVisitor.CompileCore(
                    _parseTree as CoreSqlContext,
                    context);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Context {_parseTree.GetType().FullName} is allowed.");
            }
        }

        public static EmitFunction EmiterFromId(SdmapCompilerContext context, string id, string ns)
        {
            return context.GetEmiter(id, ns)
                .Emiter;
        }
    }
}
