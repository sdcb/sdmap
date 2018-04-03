using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using sdmap.Parser.Visitor;

namespace sdmap.Macros.Implements
{
    internal static class MacroUtil
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
            var mainAttr = method.GetCustomAttribute<MacroAttribute>();
            var argsAttr = method.GetCustomAttribute<MacroArgumentsAttribute>();

            return new Macro
            {
                Name = mainAttr?.Name ?? method.Name,
                SkipArgumentRuntimeCheck = mainAttr?.SkipArgumentRuntimeCheck ?? true,
                Arguments = argsAttr?.Arguments ?? new SdmapTypes[0],                
                Method = (MacroDelegate)method.CreateDelegate(typeof(MacroDelegate))
            };
        }

        public static Result RuntimeCheck(object[] arguments, Macro macro)
        {
            if (macro.SkipArgumentRuntimeCheck)
                return Result.Ok();

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
                        if (!(arg is double))
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
                    case SdmapTypes.Sql:
                        if (!(arg is EmitFunction))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.StringOrSql:
                        if (!(arg is string) && !(arg is EmitFunction))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.Bool:
                        if (!(arg is bool))
                            return TypeCheckFail(macro, i, arg, mac);
                        break;
                    case SdmapTypes.Any:
                        // allow any types.
                        break;
                    default:
                        return TypeCheckFail(macro, i, arg, mac);
                }
            }

            return Result.Ok();
        }

        private static Result TypeCheckFail(Macro macro, int i, object arg, SdmapTypes mac)
        {
            return Result.Fail($"Macro '{macro.Name}' " +
                $"argument {i + 1} requires {mac} but provides {arg.GetType().Name}.");
        }

        public static Result<string> EvalToString(object value, OneCallContext context, object self)
        {
            if (value is string)
                return Result.Ok((string)value);
            if (value is EmitFunction)
                return ((EmitFunction)value)(context.DigNewFragments(self));
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
