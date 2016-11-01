using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros.Implements
{
    public static class CommonMacros
    {
        [MacroName("include")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Include(SdmapContext context, object self, object[] arguments)
        {
            var contextId = (string)arguments[0];
            return context.TryGetEmiter(contextId)
                .OnSuccess(emiter => emiter.Emit(self, context));
        }
    }
}
