using sdmap.Functional;
using sdmap.Macros.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace sdmap.Macros.Implements
{
    public class RuntimeMacros
    {
        public delegate Result<string> UnnamedSql(object self);

        public Result<string> val(object self)
        {
            return Result.Ok(self?.ToString() ?? "");
        }
        
        public Result<string> prop(object self, string prop)
        {
            return Result.Ok(GetPropValue(self, prop)?.ToString() ?? "");
        }


        public Result<string> iif(object self, bool condition, string t1, string t2)
            => Result.Ok(condition ? t1 : t2);

        public Result<string> iif(object self, bool condition, UnnamedSql t1, UnnamedSql t2)
            => condition ? t1(self) : t2(self);

        public Result<string> iif(object self, bool condition, string t1, UnnamedSql t2)
            => condition ? Result.Ok(t1) : t2(self);

        public Result<string> iif(object self, bool condition, UnnamedSql t1, string t2)
            => condition ? t1(self) : Result.Ok(t2);

        //[Macro("hasProp")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> HasProp(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop != null)
        //        return MacroUtil.EvalToString(arguments[1], context, self);

        //    return Empty;
        //}

        //[Macro("hasNoProp")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> HasNoProp(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null)
        //        return MacroUtil.EvalToString(arguments[1], context, self);

        //    return Empty;
        //}


        //[Macro("isEmpty")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> IsEmpty(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(prop);

        //    if (IsEmpty(GetPropValue(self, arguments[0])))
        //        return MacroUtil.EvalToString(arguments[1], context, self);
        //    return Empty;
        //}


        //[Macro("isNotEmpty")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> IsNotEmpty(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(prop);

        //    if (!IsEmpty(GetPropValue(self, arguments[0])))
        //        return MacroUtil.EvalToString(arguments[1], context, self);
        //    return Empty;
        //}


        //[Macro("isNull")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> IsNull(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Result.Ok(string.Empty);

        //    if (GetPropValue(self, arguments[0]) == null)
        //        return MacroUtil.EvalToString(arguments[1], context, self);

        //    return Empty;
        //}


        //[Macro("isNotNull")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.StringOrSql)]
        //public static Result<string> IsNotNull(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Empty;

        //    if (GetPropValue(self, arguments[0]) != null)
        //        return MacroUtil.EvalToString(arguments[1], context, self);

        //    return Empty;
        //}


        //[Macro("isEqual")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Any, SdmapTypes.StringOrSql)]
        //public static Result<string> IsEqual(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Empty;

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = arguments[1];
        //    if (IsEqual(val, compare))
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isNotEqual")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Any, SdmapTypes.StringOrSql)]
        //public static Result<string> IsNotEqual(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Empty;

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = arguments[1];
        //    if (!IsEqual(val, compare))
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isLessThan")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        //public static Result<string> IsLessThan(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(arguments[0]);

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = Convert.ToDouble(arguments[1]);
        //    if (Convert.ToDouble(val) < compare)
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isGreaterThan")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        //public static Result<string> IsGreaterThan(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(arguments[0]);

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = Convert.ToDouble(arguments[1]);
        //    if (Convert.ToDouble(val) > compare)
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isLessEqual")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        //public static Result<string> IsLessEqual(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(arguments[0]);

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = Convert.ToDouble(arguments[1]);
        //    if (Convert.ToDouble(val) <= compare)
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isGreaterEqual")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.Number, SdmapTypes.StringOrSql)]
        //public static Result<string> IsGreaterEqual(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(arguments[0]);

        //    var val = GetPropValue(self, arguments[0]);
        //    var compare = Convert.ToDouble(arguments[1]);
        //    if (Convert.ToDouble(val) >= compare)
        //        return MacroUtil.EvalToString(arguments[2], context, self);

        //    return Empty;
        //}

        //[Macro("isLike")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        //public static Result<string> IsLike(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Empty;

        //    var val = GetPropValue(self, arguments[0]) as string;
        //    if (Regex.IsMatch(val, (string)arguments[1]))
        //    {
        //        return MacroUtil.EvalToString(arguments[2], context, self);
        //    }
        //    return Empty;
        //}

        //[Macro("isNotLike")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        //public static Result<string> IsNotLike(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();

        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return Empty;

        //    var val = GetPropValue(self, arguments[0]) as string;
        //    if (!Regex.IsMatch(val, (string)arguments[1]))
        //    {
        //        return MacroUtil.EvalToString(arguments[2], context, self);
        //    }
        //    return Empty;
        //}

        //[Macro("iterate")]
        //[MacroArguments(SdmapTypes.String, SdmapTypes.StringOrSql)]
        //public static Result<string> Iterate(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();
        //    if (!(self is IEnumerable)) return RequireType(self.GetType(), "IEnumerable");

        //    var result = new StringBuilder();
        //    foreach (var newSelf in self as IEnumerable)
        //    {
        //        if (result.Length > 0) result.Append(arguments[0]);
        //        var one = MacroUtil.EvalToString(arguments[1], context, newSelf);
        //        if (one.IsFailure) return one;
        //        result.Append(one.Value);
        //    }
        //    return Result.Ok(result.ToString());
        //}

        //[Macro("each")]
        //[MacroArguments(SdmapTypes.Syntax, SdmapTypes.String, SdmapTypes.StringOrSql)]
        //public static Result<string> Each(SdmapCompilerContext context,
        //    string ns, object self, object[] arguments)
        //{
        //    if (self == null) return RequireNotNull();
        //    var prop = GetProp(self, arguments[0]);
        //    if (prop == null) return RequirePropNotNull(arguments[0]);

        //    var val = GetPropValue(self, arguments[0]);
        //    if (val == null)
        //    {
        //        return Empty;
        //    }
        //    else if (!(val is IEnumerable))
        //    {
        //        return RequirePropType(prop, "IEnumerable");
        //    }
        //    else
        //    {
        //        var result = new StringBuilder();
        //        foreach (var newSelf in val as IEnumerable)
        //        {
        //            if (result.Length > 0) result.Append(arguments[1]);
        //            var one = MacroUtil.EvalToString(arguments[2], context, newSelf);
        //            if (one.IsFailure) return one;
        //            result.Append(one.Value);
        //        }
        //        return Result.Ok(result.ToString());
        //    }
        //}

        public static object GetPropValue(object self, string prop)
        {
            if (self is IDictionary dicSelf)
            {
                return dicSelf[prop];
            }
            else
            {
                var props = prop.Split('.');
                return props.Aggregate(self, (s, p) =>
                    s?.GetType().GetTypeInfo().GetProperty(p)?.GetValue(s));
            }
        }
    }
}
