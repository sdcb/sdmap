using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sdmap.Macros.Implements
{
    public static class CommonMacros
    {
        [Macro("include")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Include(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            var contextId = (string)arguments[0];
            return context.TryGetEmiter(contextId, ns)
                .OnSuccess(emiter => emiter.Emit(self, context));
        }


        [Macro("val")]
        public static Result<string> Val(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            return Result.Ok(self?.ToString() ?? string.Empty);
        }


        [Macro("prop")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Prop(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            return Result.Ok(prop.GetValue(self)?.ToString() ?? string.Empty);
        }


        [Macro("iif")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql, SdmapTypes.StringOrSql)]
        public static Result<string> Iif(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            if (prop.PropertyType != typeof(bool) && prop.PropertyType != typeof(bool?))
                return RequirePropType(arguments[0], "bool", prop);

            var test = (bool?)prop.GetValue(self) ?? false;
            return test ?
                MacroUtil.EvalToString(arguments[1], context, self) :
                MacroUtil.EvalToString(arguments[2], context, self);
        }

        [Macro("hasProp")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> HasProp(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop != null)
                return MacroUtil.EvalToString(arguments[1], context, self);
            
            return Empty;
        }


        [Macro("isEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsEmpty(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Result.Ok(string.Empty);

            var val = prop.GetValue(self);
            if (val == null)
            {
                return MacroUtil.EvalToString(arguments[1], context, self);
            }
            else if (val is string)
            {
                if (string.IsNullOrWhiteSpace((string)val))
                    return MacroUtil.EvalToString(arguments[1], context, self);
            }
            else if (val is IEnumerable)
            {
                if (!ArrayEmpty(val))
                    return MacroUtil.EvalToString(arguments[1], context, self);
            }

            return Empty;
        }


        [Macro("isNotEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotEmpty(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = prop.GetValue(self);
            if (val == null)
            {
                return Empty;
            }
            else if (val is string)
            {
                if (string.IsNullOrWhiteSpace((string)val))
                    return Empty;
            }
            else if (val is IEnumerable)
            {
                if (ArrayEmpty(val))
                    return Empty;
            }

            return MacroUtil.EvalToString(arguments[1], context, self);
        }


        [Macro("isNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNull(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Result.Ok(string.Empty);

            if (prop.GetValue(self) == null)
            {
                return MacroUtil.EvalToString(arguments[1], context, self);
            }

            return Result.Ok(string.Empty);
        }


        [Macro("isNotNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotNull(SdmapCompilerContext context, 
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            if (prop.GetValue(self) == null)
            {
                return Result.Ok(string.Empty);
            }

            return MacroUtil.EvalToString(arguments[1], context, self);
        }


        [Macro("isEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> IsEqual(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = prop.GetValue(self) as string;
            var compare = (string)arguments[1];
            if (val == compare)
            {
                return MacroUtil.EvalToString(arguments[2], context, self);
            }
            return Empty;
        }

        [Macro("isNotEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotEqual(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = prop.GetValue(self) as string;
            var compare = (string)arguments[1];
            if (val != compare)
            {
                return MacroUtil.EvalToString(arguments[2], context, self);
            }
            return Empty;
        }

        [Macro("isLike")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> IsLike(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = prop.GetValue(self) as string;
            if (Regex.IsMatch(val, (string)arguments[1]))
            {
                return MacroUtil.EvalToString(arguments[2], context, self);
            }
            return Empty;
        }

        [Macro("isNotLike")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotLike(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = prop.GetValue(self) as string;
            if (!Regex.IsMatch(val, (string)arguments[1]))
            {
                return MacroUtil.EvalToString(arguments[2], context, self);
            }
            return Empty;
        }

        private static readonly Result<string> Empty = Result.Ok("");

        private static Result<string> RequireNotNull([CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query requires not null in macro '{caller}'.");
        }

        private static Result<string> RequirePropNotNull(object prop, [CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query requires property '{prop}' in macro '{caller}'.");
        }

        private static Result<string> RequirePropType(object syntax, string expected, PropertyInfo prop, 
            [CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query property '{syntax}' expect type '{expected}' " +
                    $"but given '{prop.PropertyType.FullName}' in macro 'iif'.");
        }

        private static PropertyInfo GetProp(object self, object syntax)
        {
            return self
                .GetType()
                .GetTypeInfo()
                .GetProperty((string)syntax);
        }

        private static bool ArrayEmpty(object arr)
        {
            return ((IEnumerable)arr).GetEnumerator().MoveNext();
        }
    }
}
