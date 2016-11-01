using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Macros.Implements;
using sdmap.Parser.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static sdmap.Macros.MacroUtil;

namespace sdmap.Macros
{
    public class SdmapMacroManager
    {
        private readonly Dictionary<string, SdmapMacro> _methods = new Dictionary<string, SdmapMacro>();

        public SdmapMacroManager()
        {
            AddImplements(typeof(CommonMacros));
        }

        public void AddImplements(Type type)
        {
            var methods = GetTypeAvailableMethods(type)
                .Select(ToSdmapMacro);

            foreach (var method in methods)
            {
                _methods.Add(method.Name, method);
            }
        }

        public Result<string> Execute(string name, SdmapContext context, object[] arguments)
        {
            SdmapMacro macro;
            if (!_methods.TryGetValue(name, out macro))
            {
                return Result.Fail<string>($"Macro: '{name}' cannot be found.");
            }

            return macro.Function(context, arguments);
        }
    }
}
