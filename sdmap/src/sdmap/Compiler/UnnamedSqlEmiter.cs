using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using sdmap.Functional;
using sdmap.Parser.Visitor;
using sdmap.Utils;
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
                var coreSql = (_parseTree as UnnamedSqlContext).coreSql();
                var fullName = NameUtil.GetFunctionName(coreSql);
                return CoreSqlVisitor.CompileCore(
                    coreSql,
                    context, 
                    fullName);
            }
            else if (_parseTree is CoreSqlContext)
            {
                var coreSql = _parseTree as CoreSqlContext;
                var fullName = NameUtil.GetFunctionName(coreSql);
                return CoreSqlVisitor.CompileCore(
                    coreSql,
                    context, 
                    fullName);
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
