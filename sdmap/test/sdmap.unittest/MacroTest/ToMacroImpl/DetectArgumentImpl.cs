using sdmap.Functional;
using sdmap.Macros;
using sdmap.Macros.Attributes;
using sdmap.Compiler;

namespace sdmap.unittest.MacroTest.ToMacroImpl
{
    public static class DetectArgumentImpl
    {
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Sql)]
        public static Result<string> DetectMe(OneCallContext context, string ns, object self, object[] arguments)
        {
            return Result.Ok("Hello World");
        }
    }
}
