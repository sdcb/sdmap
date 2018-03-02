using sdmap.Functional;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.unittest.MacroTest.FilterMethodsImpl
{
    public class ReturnCheckImpl
    {
        public static string NotCorrect(ParentEmiterContext context, string ns, object self, object[] arguments)
        {
            return "Hello World";
        }

        public static Result<string> Ok(ParentEmiterContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
