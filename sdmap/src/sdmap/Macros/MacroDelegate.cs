using sdmap.Functional;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros
{
    public delegate Result<string> MacroDelegate(SdmapContext context, 
        string ns, object self, object[] arguments);
}
