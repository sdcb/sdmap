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
        public UnnamedSqlEmiter(UnnamedSqlContext parseTree, string ns)
            : base(parseTree, ns)
        {
        }

        public static UnnamedSqlEmiter Create(UnnamedSqlContext parseTree, string ns)
        {
            return new UnnamedSqlEmiter(parseTree, ns);
        }

        protected override Result<EmitFunction> Compile(SdmapCompilerContext context)
        {
            return UnnamedSqlVisitor.Compile((UnnamedSqlContext)_parseTree, context);
        }

        public static EmitFunction EmiterFromId(SdmapCompilerContext context, string id, string ns)
        {
            return context.GetEmiter(id, ns)
                .Emiter;
        }
    }
}
