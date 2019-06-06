using sdmap.Functional;

namespace sdmap.Emiter.Implements.CSharp
{
    public interface ISdmapEmiter
    {
        Result<string> BuildText(dynamic self);
    }
}
