using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sdmap.Emiter
{
    public class CodeEmiter
    {
        public Result Emit(string source, TextWriter writer, CodeEmiterProvider codeEmiterProvider)
        {
            return codeEmiterProvider.Emit(source, writer);
        }
    }
}
