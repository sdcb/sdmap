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
using System.Text;

namespace sdmap.Macros.Implements
{
    internal static class CommonMacros
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

            return Result.Ok(GetPropValue(self, arguments[0])?.ToString() ?? string.Empty);
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
                return RequirePropType(prop, "bool");

            var test = (bool?)GetPropValue(self, arguments[0]) ?? false;
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
            if (prop == null) return RequirePropNotNull(prop);

            if (IsEmpty(GetPropValue(self, arguments[0])))
                return MacroUtil.EvalToString(arguments[1], context, self);
            return Empty;
        }


        [Macro("isNotEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotEmpty(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(prop);

            if (!IsEmpty(GetPropValue(self, arguments[0])))
                return MacroUtil.EvalToString(arguments[1], context, self);
            return Empty;
        }


        [Macro("isNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNull(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Result.Ok(string.Empty);

            if (GetPropValue(self, arguments[0]) == null)
                return MacroUtil.EvalToString(arguments[1], context, self);

            return Empty;
        }


        [Macro("isNotNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotNull(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            if (GetPropValue(self, arguments[0]) != null)
                return MacroUtil.EvalToString(arguments[1], context, self);

            return Empty;
        }


        [Macro("isEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Any, SdmapTypes.StringOrSql)]
        public static Result<string> IsEqual(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = GetPropValue(self, arguments[0]);
            var compare = arguments[1];
            if (IsEqual(val, compare))
                return MacroUtil.EvalToString(arguments[2], context, self);
            
            return Empty;
        }

        [Macro("isNotEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Any, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotEqual(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return Empty;

            var val = GetPropValue(self, arguments[0]);
            var compare = arguments[1];
            if (!IsEqual(val, compare))
                return MacroUtil.EvalToString(arguments[2], context, self);

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

            var val = GetPropValue(self, arguments[0]) as string;
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

            var val = GetPropValue(self, arguments[0]) as string;
            if (!Regex.IsMatch(val, (string)arguments[1]))
            {
                return MacroUtil.EvalToString(arguments[2], context, self);
            }
            return Empty;
        }

        [Macro("iterate")]
        [MacroArguments(SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> Iterate(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();
            if (!(self is IEnumerable)) return RequireType(self.GetType(), "IEnumerable");

            var result = new StringBuilder();
            foreach (var newSelf in self as IEnumerable)
            {
                if (result.Length > 0) result.Append(arguments[0]);
                var one = MacroUtil.EvalToString(arguments[1], context, newSelf);
                if (one.IsFailure) return one;
                result.Append(one.Value);
            }
            return Result.Ok(result.ToString());
        }

        [Macro("each")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> Each(SdmapCompilerContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();
            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            var val = GetPropValue(self, arguments[0]);
            if (val == null)
            {
                return Empty;
            }
            else if (!(val is IEnumerable))
            {
                return RequirePropType(prop, "IEnumerable");
            }
            else
            {
                var result = new StringBuilder();
                foreach (var newSelf in val as IEnumerable)
                {
                    if (result.Length > 0) result.Append(arguments[1]);
                    var one = MacroUtil.EvalToString(arguments[2], context, newSelf);
                    if (one.IsFailure) return one;
                    result.Append(one.Value);
                }
                return Result.Ok(result.ToString());
            }
        }

        private static readonly Result<string> Empty = Result.Ok("");

        private static Result<string> RequireNotNull([CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query requires not null in macro '{caller}'.");
        }

        private static Result<string> RequirePropNotNull(object prop,
            [CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query requires property '{prop}' in macro '{caller}'.");
        }

        private static Result<string> RequirePropType(PropertyInfo prop, string expected,
            [CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query property '{prop.Name}' expect type '{expected}' " +
                    $"but given '{prop.PropertyType.FullName}' in macro '{caller}'.");
        }

        private static Result<string> RequireType(Type type, string expected,
            [CallerMemberName] string caller = null)
        {
            return Result.Fail<string>($"Query object expect type '{expected}' " +
                    $"but given '{type.FullName}' in macro '{caller}'.");
        }

        public static PropertyInfo GetProp(object self, object syntax)
        {
            var props = (syntax as string).Split('.');
            var fronts = props.Take(props.Length - 1);

            var frontValue = fronts.Aggregate(self, (s, p) =>
                s?.GetType().GetTypeInfo().GetProperty(p)?.GetValue(s));
            
            return frontValue
                ?.GetType()
                .GetTypeInfo()
                .GetProperty(props.Last());
        }

        public static object GetPropValue(object self, object syntax)
        {
            var props = (syntax as string).Split('.');
            return props.Aggregate(self, (s, p) =>
                s?.GetType().GetTypeInfo().GetProperty(p)?.GetValue(s));
        }

        public static bool ArrayEmpty(object arr)
        {
            return !((IEnumerable)arr).GetEnumerator().MoveNext();
        }

        public static bool IsEqual(object val1, object val2)
        {            
            if (val2 is string)
            {
                return (val2 as string).Equals(val1);
            }
            else if (val2 is bool)
            {
                return (val2 as bool?).Equals(val1);
            }
            else if (val2 is double)
            {
                return Convert.ToDecimal(val2).Equals(val1);
            }
            else if (val2 is DateTime)
            {
                return (val2 as DateTime?).Equals(val1);
            }
            return false;
        }

        public static bool IsEmpty(object v)
        {
            if (v == null)
                return true;
            if (v is string)
                return string.IsNullOrWhiteSpace((string)v);
            if (v is IEnumerable)
                return ArrayEmpty(v);
            return false;
        }
    }
}
