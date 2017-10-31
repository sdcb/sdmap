using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Emiter.Implements.CSharp
{
    public class CSharpDefine
    {
        public string[] CommonUsings()
        {
            return new []
            {
                "System",
                "System.Text",      // for StringBuilder
                "sdmap.Functional", // for Result<T>
                "sdmap.Emiter",     // for ISdmapEmiter
            };
        }
    }
}
