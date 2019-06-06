using sdmap.Functional;
using sdmap.Compiler;

namespace sdmap.Macros
{
    public delegate Result<string> MacroDelegate(OneCallContext context, 
        string ns, object self, object[] arguments);
}
