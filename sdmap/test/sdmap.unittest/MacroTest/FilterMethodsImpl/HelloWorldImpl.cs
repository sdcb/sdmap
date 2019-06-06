using sdmap.Functional;
using sdmap.Compiler;

namespace sdmap.unittest.MacroTest.FilterMethodsImpl
{
    public static class HelloWorldImpl
    {
        public static Result<string> HelloWorld(OneCallContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
