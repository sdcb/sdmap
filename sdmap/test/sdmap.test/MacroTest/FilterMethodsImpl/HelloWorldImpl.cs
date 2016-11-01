using sdmap.Functional;
using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.test.MacroTest.FilterMethodsImpl
{
    public static class HelloWorldImpl
    {
        public static Result<string> HelloWorld(SdmapContext context, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
