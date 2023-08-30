using System;
using System.Linq;
using System.Reflection;
// ReSharper disable SuggestBaseTypeForParameter

namespace sdmap.Macros.Implements;

[Flags]
public enum ComparisonResult : byte
{
    Incomparable  = 1,
    AreEqual      = 2,
    LeftIsGreater = 4,
    LeftIsLess    = 8
}

internal static class ValueComparer
{
    public static ComparisonResult Compare(object left, object right)
        => CompareImpl(left, right) switch
        {
            null => ComparisonResult.Incomparable,
            0    => ComparisonResult.AreEqual,
            > 0  => ComparisonResult.LeftIsGreater,
            < 0  => ComparisonResult.LeftIsLess
        };
    
    private static int? CompareImpl(object left, object right)
        => left switch
        {
            null => right is null ? 0 : null,
            not null when right is null => null,

            byte        byteValue    => byteValue.CompareTo(Convert.ToByte(right)),
            sbyte       sByteValue   => sByteValue.CompareTo(Convert.ToSByte(right)),
            short       shortValue   => shortValue.CompareTo(Convert.ToInt16(right)),
            ushort      ushortValue  => ushortValue.CompareTo(Convert.ToUInt16(right)),

            int         intValue     => intValue.CompareTo(Convert.ToInt32(right)),
            uint        uIntValue    => uIntValue.CompareTo(Convert.ToUInt32(right)),
            long        longValue    => longValue.CompareTo(Convert.ToInt64(right)),
            ulong       uLongValue   => uLongValue.CompareTo(Convert.ToUInt64(right)),

            float       floatValue   => floatValue.CompareTo(Convert.ToSingle(right)),
            double      doubleValue  => doubleValue.CompareTo(Convert.ToDouble(right)),
            decimal     decimalValue => decimalValue.CompareTo(Convert.ToDecimal(right)),

            Enum        enumValue    => Compare(enumValue, right),
            IComparable comparable   => Compare(comparable, right),

            _ => null
        };

    private static int? Compare(Enum leftEnum, object right)
    {
        if (right is Enum rightEnum)
        {
            return leftEnum.GetType() == rightEnum.GetType()
                ? leftEnum.CompareTo(rightEnum)
                : null;
        }

        return EnumParser.TryParse(leftEnum.GetType(), right.ToString(), out var parsed)
            ? leftEnum.CompareTo(parsed)
            : null;
    }

    private static int? Compare(IComparable comparable, object right)
    {
        var leftType = comparable.GetType();

        if (leftType == right.GetType())
        {
            return comparable.CompareTo(right);
        }

        return TryParse(leftType, right.ToString(), out var parsed)
            ? comparable.CompareTo(parsed)
            : null;

        static bool TryParse(Type targetType, string input, out object parsed)
        {
            if (targetType == typeof(Guid))
            {
                var result = Guid.TryParse(input, out var parsedValue);
                parsed = parsedValue;
                return result;
            }

            if (targetType == typeof(TimeSpan))
            {
                var result = TimeSpan.TryParse(input, out var parsedValue);
                parsed = parsedValue;
                return result;
            }

            if (targetType == typeof(DateTime))
            {
                var result = DateTime.TryParse(input, out var parsedValue);
                parsed = parsedValue;
                return result;
            }

            if (targetType == typeof(DateTimeOffset))
            {
                var result = DateTimeOffset.TryParse(input, out var parsedValue);
                parsed = parsedValue;
                return result;
            }

            parsed = null;
            return false;
        }
    }

    private static class EnumParser
    {
        private static readonly MethodInfo TryParseMethod
            = typeof(Enum)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(method =>
                    method.Name == nameof(Enum.TryParse)
                    && method.IsGenericMethodDefinition
                    && method.ReturnType == typeof(bool)
                    && method.GetParameters().Length == 3
                    && method.GetParameters().First().ParameterType == typeof(string)
                );

        public static bool TryParse(Type enumType, string value, out object parsed)
        {
            var parameters = new object[] { value, true, null };

            var result = TryParseMethod
                .MakeGenericMethod(enumType)
                .Invoke(obj: null, parameters);

            parsed = parameters[2];
            return (bool)result;
        }
    }
}