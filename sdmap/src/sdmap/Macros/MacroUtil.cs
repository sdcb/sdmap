using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace sdmap.Macros
{
    public static class MacroUtil
    {
        public static IEnumerable<MethodInfo> GetTypeMacroMethods(Type type)
        {
            var target = typeof(MacroDelegate).GetTypeInfo().GetMethod("Invoke");

            return type
                .GetTypeInfo()
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method =>
                {
                    return
                        Enumerable.SequenceEqual(
                            method.GetParameters().Select(x => x.ParameterType),
                            target.GetParameters().Select(x => x.ParameterType)) &&
                        method.ReturnType == target.ReturnType;
                });
        }

        public static Macro ToSdmapMacro(MethodInfo method)
        {
            var nameAttr = method.GetCustomAttribute<MacroNameAttribute>();
            var argsAttr = method.GetCustomAttribute<MacroArgumentsAttribute>();

            return new Macro
            {
                Name = nameAttr?.Name ?? method.Name,
                Arguments = argsAttr?.Arguments ?? new SdmapTypes[0],
                Function = (MacroDelegate)method.CreateDelegate(typeof(MacroDelegate))
            };
        }

        public static Result RuntimeCheck(object[] arguments, Macro macro)
        {
            if ((arguments?.Length ?? 0) != macro.Arguments.Length)
            {
                return Result.Fail($"Macro '{macro.Name}' need" +
                    $"{macro.Arguments.Length} arguments but provides {arguments?.Length ?? 0}.");
            }

            for (var i = 0; i < macro.Arguments.Length; ++i)
            {
                var arg = arguments[i];
                var mac = macro.Arguments[i];

                switch (mac)
                {
                    case SdmapTypes.Date:
                        if (!(arg is DateTime))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.Number:
                        if (!(arg is decimal || 
                            arg is int || arg is short || arg is long || 
                            arg is uint || arg is ushort || arg is ulong ||
                            arg is float || arg is double))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.String:
                        if (!(arg is string))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.Syntax:
                        if (!(arg is string))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.UnnamedSql:
                        if (!(arg is EmitFunction))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                }
            }

            return Result.Ok();
        }

        private static Result TypeCheckFail(Macro macro, int i, object arg, SdmapTypes mac)
        {
            return Result.Fail($"Macro '{macro.Name}' " +
                $"argument {i + 1} requires {mac} but provides {arg.GetType().Name}.");
        }
    }
}
