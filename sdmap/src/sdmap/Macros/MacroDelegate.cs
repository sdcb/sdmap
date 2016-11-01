using sdmap.Functional;
using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros
{
    public delegate Result<string> SdmapMacroDelegate(SdmapContext context, object[] arguments);
}
