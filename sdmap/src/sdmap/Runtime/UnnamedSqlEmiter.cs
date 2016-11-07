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
        public UnnamedSqlEmiter(UnnamedSqlContext parseTree)
            : base(parseTree)
        {
        }

        public static UnnamedSqlEmiter Create(UnnamedSqlContext parseTree)
        {
            return new UnnamedSqlEmiter(parseTree);
        }

        public override Result<EmitFunction> Compile(SdmapContext context)
        {
            return UnnamedSqlVisitor.Compile((UnnamedSqlContext)_parseTree, context);
        }
    }
}
