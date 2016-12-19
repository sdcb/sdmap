using sdmap.Functional;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.FilterMethodsImpl
{
    public static class HelloWorldImpl
    {
        public static Result<string> HelloWorld(SdmapContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
