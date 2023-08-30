using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace sdmap.Macros.Implements;

internal class ValueComparer : IComparer<object>
{
    private ValueComparer()
    {
    }
    
    public static IComparer<object> Instance { get; } = new ValueComparer();

    public int Compare(object x, object y)
    {
        throw new NotImplementedException();
    }
    
    public static bool IsEqual(object v1, object v2)
        => v1 switch
        {
            null                     => v2 is null,
            not null when v2 is null => false,

            byte    byteValue    => byteValue.Equals(Convert.ToByte(v2)),
            sbyte   sByteValue   => sByteValue.Equals(Convert.ToSByte(v2)),
            short   shortValue   => shortValue.Equals(Convert.ToInt16(v2)),
            ushort  ushortValue  => ushortValue.Equals(Convert.ToUInt16(v2)),

            int     intValue     => intValue.Equals(Convert.ToInt32(v2)),
            uint    uIntValue    => uIntValue.Equals(Convert.ToUInt32(v2)),
            long    longValue    => longValue.Equals(Convert.ToInt64(v2)),
            ulong   uLongValue   => uLongValue.Equals(Convert.ToUInt64(v2)),

            float   floatValue   => floatValue.Equals(Convert.ToSingle(v2)),
            double  doubleValue  => doubleValue.Equals(Convert.ToDouble(v2)),
            decimal decimalValue => decimalValue.Equals(Convert.ToDecimal(v2)),

            Enum enumValue
                => v2 is Enum other
                    ? enumValue.Equals(other)
                    : EnumParser.TryParse(enumValue.GetType(), v2.ToString(), out var parsed)
                      && enumValue.Equals(parsed),

            Guid guid
                => v2 is Guid other
                    ? guid.Equals(other)
                    : Guid.TryParse(v2.ToString(), out var parsed) && guid.Equals(parsed),

            TimeSpan timeSpan
                => v2 is TimeSpan other
                    ? timeSpan.Equals(other)
                    : TimeSpan.TryParse(v2.ToString(), out var parsed) && timeSpan.Equals(parsed),

            DateTime dateTime
                => v2 is DateTime other
                    ? dateTime.Equals(other)
                    : DateTime.TryParse(v2.ToString(), out var parsed) && dateTime.Equals(parsed),

            DateTimeOffset dateTimeOffset
                => v2 is DateTimeOffset other
                    ? dateTimeOffset.Equals(other)
                    : DateTimeOffset.TryParse(v2.ToString(), out var parsed) && dateTimeOffset.Equals(parsed),

            _ => v1.Equals(v2)
        };

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