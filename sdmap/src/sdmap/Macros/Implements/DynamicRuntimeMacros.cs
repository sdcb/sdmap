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
    internal class DynamicRuntimeMacros
    {
        [Macro("include")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Include(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            var contextId = (string)arguments[0];
            return context.Compiler.TryGetEmiter(contextId, ns)
                .OnSuccess(emiter => emiter.Emit(context.Dig(self)));
        }


        [Macro("val")]
        public static Result<string> Val(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            return Result.Ok(self?.ToString() ?? string.Empty);
        }


        [Macro("prop")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Prop(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            return Result.Ok(GetPropValue(self, arguments[0])?.ToString() ?? string.Empty);
        }


        [Macro("iif")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql, SdmapTypes.StringOrSql)]
        public static Result<string> Iif(OneCallContext context,
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
        public static Result<string> HasProp(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop != null)
                return MacroUtil.EvalToString(arguments[1], context, self);

            return Empty;
        }

        [Macro("hasNoProp")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> HasNoProp(OneCallContext context, 
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null)
                return MacroUtil.EvalToString(arguments[1], context, self);

            return Empty;
        }


        [Macro("isEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsEmpty(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            if (IsEmpty(GetPropValue(self, arguments[0])))
                return MacroUtil.EvalToString(arguments[1], context, self);
            return Empty;
        }


        [Macro("isNotEmpty")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNotEmpty(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            if (!IsEmpty(GetPropValue(self, arguments[0])))
                return MacroUtil.EvalToString(arguments[1], context, self);
            return Empty;
        }


        [Macro("isNull")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> IsNull(OneCallContext context,
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
        public static Result<string> IsNotNull(OneCallContext context,
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
        public static Result<string> IsEqual(OneCallContext context,
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
        public static Result<string> IsNotEqual(OneCallContext context,
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

        [Macro("isLessThan")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        public static Result<string> IsLessThan(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            var val = GetPropValue(self, arguments[0]);
            var compare = Convert.ToDouble(arguments[1]);
            if (Convert.ToDouble(val) < compare)
                return MacroUtil.EvalToString(arguments[2], context, self);

            return Empty;
        }

        [Macro("isGreaterThan")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        public static Result<string> IsGreaterThan(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            var val = GetPropValue(self, arguments[0]);
            var compare = Convert.ToDouble(arguments[1]);
            if (Convert.ToDouble(val) > compare)
                return MacroUtil.EvalToString(arguments[2], context, self);

            return Empty;
        }

        [Macro("isLessEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        public static Result<string> IsLessEqual(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            var val = GetPropValue(self, arguments[0]);
            var compare = Convert.ToDouble(arguments[1]);
            if (Convert.ToDouble(val) <= compare)
                return MacroUtil.EvalToString(arguments[2], context, self);

            return Empty;
        }

        [Macro("isGreaterEqual")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        public static Result<string> IsGreaterEqual(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            if (self == null) return RequireNotNull();

            var prop = GetProp(self, arguments[0]);
            if (prop == null) return RequirePropNotNull(arguments[0]);

            var val = GetPropValue(self, arguments[0]);
            var compare = Convert.ToDouble(arguments[1]);
            if (Convert.ToDouble(val) >= compare)
                return MacroUtil.EvalToString(arguments[2], context, self);

            return Empty;
        }

        [Macro("isLike")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        public static Result<string> IsLike(OneCallContext context,
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
        public static Result<string> IsNotLike(OneCallContext context,
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
        public static Result<string> Iterate(OneCallContext context,
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
        public static Result<string> Each(OneCallContext context,
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

        [Macro("def")]
        [MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        public static Result<string> Def(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            var id = (string)arguments[0];
            if (arguments[1] is string sql)
            {
                var fragment = new RawSegmentDef(id, sql);
                context.Fragments.Add(fragment);
                context.Defs.Add(fragment);
            }
            else if (arguments[1] is EmitFunction emiter)
            {
                var fragment = new EmiterSegmentDef(id, emiter);
                context.Fragments.Add(fragment);
                context.Defs.Add(fragment);
            }
            else
            {
                return Result.Fail<string>("argument 1 must be string or sql statement.");
            }
            return Empty;
        }

        [Macro("deps", SkipArgumentRuntimeCheck = true)]
        public static Result<string> Deps(OneCallContext context,
            string ns, object self, object[] arguments)
        {
            foreach (var dep in arguments.Select(x => (string)x))
            {
                context.Deps.Add(dep);
            }
            return Result.Ok(string.Empty);
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

        private static Result<string> RequirePropType(QueryPropertyInfo prop, string expected,
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

        public static QueryPropertyInfo GetProp(object self, object syntax)
        {
            var props = (syntax as string).Split('.');
            var fronts = props.Take(props.Length - 1);

            if (self is IDictionary dicSelf)
            {
                if (!dicSelf.Contains(syntax))
                    return null;

                var val = dicSelf[syntax];
                if (val == null)
                    return new QueryPropertyInfo(props[0], typeof(object));

                return new QueryPropertyInfo(props[0], val.GetType());
            }
            else
            {
                var frontValue = fronts.Aggregate(self, (s, p) =>
                    s?.GetType().GetTypeInfo().GetProperty(p)?.GetValue(s));

                var pi = frontValue
                    ?.GetType()
                    .GetTypeInfo()
                    .GetProperty(props.Last());
                if (pi == null) return null;

                return new QueryPropertyInfo(pi.Name, pi.PropertyType);
            }
        }

        public static object GetPropValue(object self, object syntax)
        {
            return RuntimeMacros.GetPropValue(self, (string)syntax);
        }

        public static bool IsEqual(object v1, object v2)
        {
            return RuntimeMacros.IsEqual(v1, v2);
        }

        public static bool IsEmpty(object v)
        {
            return RuntimeMacros.IsEmpty(v);
        }
    }
}
