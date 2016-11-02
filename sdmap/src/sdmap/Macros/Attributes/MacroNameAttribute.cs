using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MacroNameAttribute : Attribute
    {
        public string Name { get; }

        public MacroNameAttribute(string name)
        {
            Name = name;
        }
    }
}
