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
    public class SqlEmiter : SqlEmiterBase
    {
        public SqlEmiter(NamedSqlContext parseTree, string ns)
            : base(parseTree, ns)
        {
        }

        public static SqlEmiter Create(NamedSqlContext parseTree, string ns)
        {
            return new SqlEmiter(parseTree, ns);
        }

        protected override Result<EmitFunction> Compile(SdmapCompilerContext context)
        {
            return NamedSqlVisitor.Compile((NamedSqlContext)_parseTree, context);
        }
    }
}
