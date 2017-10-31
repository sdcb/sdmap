using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Emiter
{
    public interface ISdmapEmiter
    {
        Result<string> BuildText();
    }
}
