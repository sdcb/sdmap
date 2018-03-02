using sdmap.Functional;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.unittest.MacroTest.FilterMethodsImpl
{
    public class ParameterCheckImpl
    {
        public static Result<string> Ok(ParentEmiterContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> More(ParentEmiterContext context, string ns, object self, object[] arguments, int v)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Less(ParentEmiterContext context, string ns, object self)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Changed(ParentEmiterContext context, string ns, object self, int[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
