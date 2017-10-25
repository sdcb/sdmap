using sdmap.Emiter.Implements.Common;
using sdmap.Functional;
using System.IO;

namespace sdmap.Emiter
{
    public interface CodeEmiterProvider
    {
        Result Emit(
            string source, 
            TextWriter writer, 
            CodeEmiterConfig config);
    }
}