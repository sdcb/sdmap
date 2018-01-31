using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.ext
{
    public interface ISdmapEmiter
    {
        string Emit(string statementId, object parameters);
    }
}
