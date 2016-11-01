using sdmap.Functional;
using sdmap.Macros;
using sdmap.Macros.Attributes;
using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.ToMacroImpl
{
    public static class DetectArgumentImpl
    {
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.UnnamedSql)]
        public static Result<string> DetectMe(SdmapContext context, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
