using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Utils
{
    public static class NameUtil
    {
        public static string GetFunctionName(ParserRuleContext context)
        {
            return "UnNamed_" + HashUtil.Base64SHA256(context.GetText());
        }
    }
}
