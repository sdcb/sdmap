using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Compiler;

namespace sdmap.unittest.MacroTest.ToMacroImpl
{
    public static class NameCanChangeImpl
    {
        [Macro("NiceName")]
        public static Result<string> HelloWorld(OneCallContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
