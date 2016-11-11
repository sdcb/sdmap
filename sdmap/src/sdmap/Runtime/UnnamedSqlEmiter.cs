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

        public static Result<string> Execute(SdmapContext context, string id, object self)
        {
            return context.TryGetEmiter(id)
                .OnSuccess(v => v.TryEmit(self, context));
        }
    }
}
