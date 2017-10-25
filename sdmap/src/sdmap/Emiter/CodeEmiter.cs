using sdmap.Emiter.Implements.Common;
using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sdmap.Emiter
{
    public class CodeEmiter
    {
        public Result Emit(
            string source, TextWriter writer, CodeEmiterConfig config, CodeEmiterProvider codeEmiterProvider)
        {
            return codeEmiterProvider.Emit(source, writer, config);
        }
    }
}
