using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Utils
{
    public static class NameUtil
    {
        public static string GetFunctionName(UnnamedSqlContext context)
        {
            return "UnNamed_" + HashUtil.Base64SHA256(context.GetText());
        }

        public static string GetFunctionName(CoreSqlContext context)
        {
            return "UnNamed_" + HashUtil.Base64SHA256(context.GetText());
        }
    }
}
