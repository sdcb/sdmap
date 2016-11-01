using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Macros.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MacroArgumentsAttribute : Attribute
    {
        public int[] Arguments { get; }

        public MacroArgumentsAttribute(params int[] arguments)
        {
            Arguments = arguments;
        }
    }
}
