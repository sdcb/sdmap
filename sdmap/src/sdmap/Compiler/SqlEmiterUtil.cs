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
    public static class SqlEmiterUtil
    {
        public static SqlEmiter CreateNamed(NamedSqlContext parseTree, string ns)
        {
            return new SqlEmiter(parseTree, ns,
                (ctx, pt) => CompileNamed(ctx, (NamedSqlContext)pt));
        }

        private static Result<EmitFunction> CompileNamed(
            SdmapCompilerContext context,
            NamedSqlContext parseTree)
        {
            return NamedSqlVisitor.Compile(parseTree, context);
        }

        public static SqlEmiter CreateUnnamed(ParserRuleContext parseTree, string ns)
        {
            return new SqlEmiter(parseTree, ns, CompileUnnamed);
        }

        private static Result<EmitFunction> CompileUnnamed(
            SdmapCompilerContext context,
            ParserRuleContext parseTree)
        {
            if (parseTree is UnnamedSqlContext)
            {
                var coreSql = (parseTree as UnnamedSqlContext).coreSql();
                var fullName = NameUtil.GetFunctionName(coreSql);
                return CoreSqlVisitor.CompileCore(
                    coreSql,
                    context,
                    fullName);
            }
            else if (parseTree is CoreSqlContext)
            {
                var coreSql = parseTree as CoreSqlContext;
                var fullName = NameUtil.GetFunctionName(coreSql);
                return CoreSqlVisitor.CompileCore(
                    coreSql,
                    context,
                    fullName);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Context {parseTree.GetType().FullName} is allowed.");
            }
        }

        public static EmitFunction EmiterFromId(SdmapCompilerContext context, string id, string ns)
        {
            return context.GetEmiter(id, ns)
                .Emiter;
        }
    }
}
