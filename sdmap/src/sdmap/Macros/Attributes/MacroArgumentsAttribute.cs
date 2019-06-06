using System;

namespace sdmap.Macros.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MacroArgumentsAttribute : Attribute
    {
        public SdmapTypes[] Arguments { get; }

        public MacroArgumentsAttribute(params SdmapTypes[] arguments)
        {
            Arguments = arguments;
        }
    }
}
