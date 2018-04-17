using sdmap.Functional;
using sdmap.Macros.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace sdmap.Macros.Implements
{
    public class RuntimeMacros
    {
        public delegate Result<string> UnnamedSql(object self);

        // include
        public Result<string> include()
            => throw new InvalidOperationException(
                $"#include is implemented by compiler instead of runtime, code should never go here.");

        // val
        public Result<string> val(object self)
            => Result.Ok(self?.ToString() ?? "");
        
        // prop
        public Result<string> prop(object self, string prop)
            => Result.Ok(GetPropValue(self, prop)?.ToString() ?? "");

        // iif
        public Result<string> iif(object self, string prop, string t1, string t2)
            => Result.Ok(EvalPropToBool(self, prop) ? t1 : t2);

        public Result<string> iif(object self, string prop, UnnamedSql t1, UnnamedSql t2)
            => EvalPropToBool(self, prop) ? t1(self) : t2(self);

        public Result<string> iif(object self, string prop, string t1, UnnamedSql t2)
            => EvalPropToBool(self, prop) ? Result.Ok(t1) : t2(self);

        public Result<string> iif(object self, string prop, UnnamedSql t1, string t2)
            => EvalPropToBool(self, prop) ? t1(self) : Result.Ok(t2);

        // hasProp
        public Result<string> hasProp(object self, string prop, string text)
            => Return(HasProp(self, prop), text);

        public Result<string> hasProp(object self, string prop, UnnamedSql text)
            => Return(HasProp(self, prop), text(self));
        
        public Result<string> hasNoProp(object self, string prop, string text)
            => Return(!HasProp(self, prop), text);

        public Result<string> hasNoProp(object self, string prop, UnnamedSql text)
            => Return(!HasProp(self, prop), text(self));

        // isEmpty
        public Result<string> isEmpty(object self, string prop, string text)
            => Return(IsEmpty(GetPropValue(self, prop)), text);

        public Result<string> isEmpty(object self, string prop, UnnamedSql text)
            => Return(IsEmpty(GetPropValue(self, prop)), text(self));
        
        public Result<string> isNotEmpty(object self, string prop, string text)
            => Return(!IsEmpty(GetPropValue(self, prop)), text);

        public Result<string> isNotEmpty(object self, string prop, UnnamedSql text)
            => Return(!IsEmpty(GetPropValue(self, prop)), text(self));

        // isNull
        public Result<string> isNull(object self, string prop, string text)
            => Return(GetPropValue(self, prop) == null, text);

        public Result<string> isNull(object self, string prop, UnnamedSql text)
            => Return(GetPropValue(self, prop) == null, text(self));
        
        public Result<string> isNotNull(object self, string prop, string text)
            => Return(GetPropValue(self, prop) != null, text);

        public Result<string> isNotNull(object self, string prop, UnnamedSql text)
            => Return(GetPropValue(self, prop) != null, text(self));

        // isEqual
        public Result<string> isEqual(object self, string prop, object v, string text)
            => Return(IsEqual(GetPropValue(self, prop), v), text);

        public Result<string> isEqual(object self, string prop, object v, UnnamedSql text)
            => Return(IsEqual(GetPropValue(self, prop), v), text(self));
        
        public Result<string> isNotEqual(object self, string prop, object v, string text)
            => Return(!IsEqual(GetPropValue(self, prop), v), text);

        public Result<string> isNotEqual(object self, string prop, object v, UnnamedSql text)
            => Return(!IsEqual(GetPropValue(self, prop), v), text(self));

        // compare
        public Result<string> isLessThan(object self, string prop, object v, string text)
            => Return(Compare(GetPropValue(self, prop), v) < 0, text);

        public Result<string> isLessThan(object self, string prop, object v, UnnamedSql text)
            => Return(Compare(GetPropValue(self, prop), v) < 0, text(self));

        public Result<string> isGreaterThan(object self, string prop, object v, string text)
            => Return(Compare(GetPropValue(self, prop), v) > 0, text);

        public Result<string> isGreaterThan(object self, string prop, object v, UnnamedSql text)
            => Return(Compare(GetPropValue(self, prop), v) > 0, text(self));

        public Result<string> isLessEqual(object self, string prop, object v, string text)
            => Return(Compare(GetPropValue(self, prop), v) <= 0, text);

        public Result<string> isLessEqual(object self, string prop, object v, UnnamedSql text)
            => Return(Compare(GetPropValue(self, prop), v) <= 0, text(self));

        public Result<string> isGreaterEqual(object self, string prop, object v, string text)
            => Return(Compare(GetPropValue(self, prop), v) >= 0, text);

        public Result<string> isGreaterEqual(object self, string prop, object v, UnnamedSql text)
            => Return(Compare(GetPropValue(self, prop), v) >= 0, text(self));

        // isLike
        public Result<string> isLike(object self, string prop, string regex, string text)
            => Return(Regex.IsMatch((string)GetPropValue(self, prop), regex), text);

        public Result<string> isLike(object self, string prop, string regex, UnnamedSql text)
            => Return(Regex.IsMatch((string)GetPropValue(self, prop), regex), text(self));

        public Result<string> isNotLike(object self, string prop, string regex, string text)
            => Return(!Regex.IsMatch((string)GetPropValue(self, prop), regex), text);

        public Result<string> isNotLike(object self, string prop, string regex, UnnamedSql text)
            => Return(!Regex.IsMatch((string)GetPropValue(self, prop), regex), text(self));
        
        // iterate
        public Result<string> iterate(object self, UnnamedSql text)
        {
            var result = new StringBuilder();
            foreach (var newSelf in (IEnumerable)self)
            {
                var ok = text(newSelf);
                if (ok.IsFailure) return ok;
                result.Append(ok.Value);
            }
            return Result.Ok(result.ToString());
        }

        // each
        public Result<string> each(object self, string prop, UnnamedSql text)
        {
            var result = new StringBuilder();
            foreach (var newSelf in (IEnumerable)GetPropValue(self, prop))
            {
                var ok = text(newSelf);
                if (ok.IsFailure) return ok;
                result.Append(ok.Value);
            }
            return Result.Ok(result.ToString());
        }

        #region private stuff
        private static Result<string> Return(bool condition, string text)
            => condition ? Result.Ok(text) : Empty;

        private static Result<string> Return(bool condition, Result<string> text)
            => condition ? text : Empty;

        private static Result<string> Empty = Result.Ok("");

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

        public static int Compare(object v1, object v2)
        {
            if (v1 is IComparable cv1)
                return cv1.CompareTo(v2);
            throw new Exception($"{v1} is not comparable.");
        }

        public static bool IsEqual(object v1, object v2)
        {
            if (v1 is string str)
                return str.Equals((string)v2);

            if (v1 is bool b)
                return b.Equals((bool)v2);

            if (v1 is int integer)
                return integer.Equals(Convert.ToInt32(v2));

            if (v1 is double db)
                return db.Equals(Convert.ToDouble(v2));

            if (v1 is decimal dm)
                return dm.Equals(Convert.ToDecimal(v2));

            if (v1 is DateTime date)
                return date.Equals((DateTime)v2);

            if (v1 is Enum @enum)
            {
                if (v2 is string v2s)
                    return @enum.ToString() == v2s;
                if (v2 is double v2d)
                    return Convert.ToInt64(@enum) == v2d;
            }

            return v1 == v2;
        }

        public static bool EvalPropToBool(object self, string prop)
        {
            var val = GetPropValue(self, prop);
            if (val == null) return false;
            // should throw exception here.
            return (bool)val;
        }

        public static bool HasProp(object self, string prop)
        {
            if (self is IDictionary dicSelf)
            {
                return dicSelf.Contains(prop);
            }
            else
            {
                return self.GetType().GetTypeInfo().GetProperties()
                    .Any(x => x.Name == prop);
            }
        }

        public static bool ArrayEmpty(IEnumerable arr)
        {
            return !arr.GetEnumerator().MoveNext();
        }

        public static bool IsEmpty(object v)
        {
            if (v == null)
                return true;
            if (v is string str)
                return string.IsNullOrWhiteSpace(str);
            if (v is IEnumerable arr)
                return ArrayEmpty(arr);

            return false;
        }
        #endregion
    }
}
