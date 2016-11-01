using sdmap.Functional;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros
{
    public delegate Result<string> SdmapMacroDelegate(SdmapContext context, object self, object[] arguments);
}
