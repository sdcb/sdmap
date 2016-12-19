using sdmap.Functional;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Runtime
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

        protected override Result<EmitFunction> Compile(SdmapContext context)
        {
            return UnnamedSqlVisitor.Compile((UnnamedSqlContext)_parseTree, context);
        }

        public static EmitFunction EmiterFromId(SdmapContext context, string id)
        {
            return context.GetEmiter(id, context.CurrentNs)
                .Emiter;
        }
    }
}
