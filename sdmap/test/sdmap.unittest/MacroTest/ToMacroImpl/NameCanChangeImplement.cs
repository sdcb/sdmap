using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.unittest.MacroTest.ToMacroImpl
{
    public static class NameCanChangeImpl
    {
        [Macro("NiceName")]
        public static Result<string> HelloWorld(ParentEmiterContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
