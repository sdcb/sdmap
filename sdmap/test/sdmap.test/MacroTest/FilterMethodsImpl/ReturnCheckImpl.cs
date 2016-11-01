using sdmap.Functional;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.FilterMethodsImpl
{
    public class ReturnCheckImpl
    {
        public static string NotCorrect(SdmapContext context, object self, object[] arguments)
        {
            return "Hello World";
        }

        public static Result<string> Ok(SdmapContext context, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
