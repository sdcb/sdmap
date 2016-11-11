using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace sdmap.Macros.Implements
{
    public static class CommonMacros
    {
        [Macro("include")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Include(SdmapContext context, object self, object[] arguments)
        {
            var contextId = (string)arguments[0];
            return context.TryGetEmiter(contextId)
                .OnSuccess(emiter => emiter.Emit(self, context));
        }

        [Macro("val")]
        public static Result<string> Val(SdmapContext context, object self, object[] arguments)
        {
            return Result.Ok(self?.ToString() ?? string.Empty);
        }

        [Macro("prop")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Prop(SdmapContext context, object self, object[] arguments)
        {
            if (self == null)
                return Result.Fail<string>($"Query requires not null in macro '{nameof(Prop)}'.");
            var syntax = (string)arguments[0];
            var prop = self.GetType().GetTypeInfo().GetProperty(syntax);
            if (prop == null)
                return Result.Fail<string>($"Query requires property '{syntax}' in macro '{nameof(Prop)}'.");
            return Result.Ok(prop.GetValue(self)?.ToString() ?? string.Empty);
        }

        [Macro("iif")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql, SdmapTypes.StringOrSql)]
        public static Result<string> Iif(SdmapContext context, object self, object[] arguments)
        {
            if (self == null)
                return Result.Fail<string>(
                    "Query requires not null in macro 'prop'.");
            var syntax = (string)arguments[0];
            var prop = self.GetType().GetTypeInfo().GetProperty(syntax);
            if (prop == null)
                return Result.Fail<string>(
                    $"Query requires property '{syntax}' in macro 'prop'.");

            if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                var test = (bool?)prop.GetValue(self) ?? false;
                return test ?
                    MacroUtil.EvalToString(arguments[1], context, self) :
                    MacroUtil.EvalToString(arguments[2], context, self);
            }
            else
            {
                return Result.Fail<string>($"Query property '{syntax}' expect type bool " +
                    $"but given '{prop.PropertyType.FullName}' in macro 'iif'.");
            }
        }

        [Macro("ifNotEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IfNotEmpty(SdmapContext context, object self, object[] arguments)
        {
            if (self == null) return Result.Fail<string>(
                    $"Query requires not null in macro '{nameof(IfNotEmpty)}'.");
            var syntax = (string)arguments[0];
            var prop = self.GetType().GetTypeInfo().GetProperty(syntax);
            if (prop == null) return Result.Ok(string.Empty);

            if (prop.PropertyType == typeof(string))
            {
                if (string.IsNullOrWhiteSpace((string)prop.GetValue(self)))
                {
                    return Result.Ok(string.Empty);
                }
            }
            else if (prop.GetValue(self) == null)
            {
                return Result.Ok(string.Empty);
            }

            return MacroUtil.EvalToString(arguments[1], context, self);
        }

        [Macro("ifNotNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IfNotNull(SdmapContext context, object self, object[] arguments)
        {
            if (self == null) return Result.Fail<string>(
                    $"Query requires not null in macro '{nameof(IfNotNull)}'.");
            var syntax = (string)arguments[0];
            var prop = self.GetType().GetTypeInfo().GetProperty(syntax);
            if (prop == null) return Result.Ok(string.Empty);

            if (prop.GetValue(self) == null)
            {
                return Result.Ok(string.Empty);
            }

            return MacroUtil.EvalToString(arguments[1], context, self);
        }
    }
}
