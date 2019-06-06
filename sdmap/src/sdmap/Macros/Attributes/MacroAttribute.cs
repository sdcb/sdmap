using System;

namespace sdmap.Macros.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MacroAttribute : Attribute
    {
        public string Name { get; }

        public bool SkipArgumentRuntimeCheck { get; set; }

        public MacroAttribute(string name)
        {
            Name = name;
        }
    }
}
