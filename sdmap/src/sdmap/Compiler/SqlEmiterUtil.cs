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
                ctx => CompileNamed(ctx, parseTree));
        }

        public static Result<EmitFunction> CompileNamed(
            SdmapCompilerContext context,
            NamedSqlContext parseTree)
        {
            var id = parseTree.GetToken(SYNTAX, 0).GetText();
            var fullName = context.GetFullNameInCurrentNs(id);

            var core = new CoreSqlVisitor(context);
            return core.Process(parseTree.coreSql(), fullName)
                .OnSuccess(() => core.Function);
        }

        public static SqlEmiter CreateUnnamed(UnnamedSqlContext parseTree, string ns)
        {
            return new SqlEmiter(parseTree, ns, 
                ctx => CompileUnnamed(ctx, parseTree));
        }

        private static Result<EmitFunction> CompileUnnamed(
            SdmapCompilerContext context,
            UnnamedSqlContext unnamedSql)
        {
            return CompileCore(context, unnamedSql.coreSql());
        }

        public static SqlEmiter CreateCore(CoreSqlContext parseTree, string ns)
        {
            return new SqlEmiter(parseTree, ns,
                ctx => CompileCore(ctx, parseTree));
        }

        private static Result<EmitFunction> CompileCore(
            SdmapCompilerContext context,
            CoreSqlContext coreSql)
        {
            var fullName = NameUtil.GetFunctionName(coreSql);
            return CoreSqlVisitor.CompileCore(
                coreSql,
                context,
                fullName);
        }

        public static EmitFunction EmiterFromId(SdmapCompilerContext context, string id, string ns)
        {
            return context.GetEmiter(id, ns)
                .Emiter;
        }
    }
}
