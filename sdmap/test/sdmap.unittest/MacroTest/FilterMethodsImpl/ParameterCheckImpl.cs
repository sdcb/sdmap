using sdmap.Functional;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.FilterMethodsImpl
{
    public class ParameterCheckImpl
    {
        public static Result<string> Ok(SdmapCompilerContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> More(SdmapCompilerContext context, string ns, object self, object[] arguments, int v)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Less(SdmapCompilerContext context, string ns, object self)
        {
            return Result.Ok("Hello World");
        }

        public static Result<string> Changed(SdmapCompilerContext context, string ns, object self, int[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
