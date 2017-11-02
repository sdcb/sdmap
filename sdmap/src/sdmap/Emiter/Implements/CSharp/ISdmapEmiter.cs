using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Emiter.Implements.CSharp
{
    public interface ISdmapEmiter
    {
        Result<string> BuildText(dynamic self);
    }
}
