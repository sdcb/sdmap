using sdmap.Functional;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.FilterMethodsImpl
{
    public class ParameterCheckImpl
    {
        public static Result<string> Ok(SdmapContext context, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> More(SdmapContext context, object self, object[] arguments, int v)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Less(SdmapContext context, object self)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Changed(SdmapContext context, object self, int[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
