using System;
using System.Collections;
using System.Linq;

namespace sdmap.Macros.Implements;

internal readonly record struct PropertyMetadata(string Name, object Value, Type Type)
{
    public static PropertyMetadata Empty => new(string.Empty, default, typeof(object));
}

internal static class PropertyMetadataRetriever
{
    public static PropertyMetadata Get(object target, string propertyAccess)
    {
        if (string.IsNullOrWhiteSpace(propertyAccess))
        {
            return PropertyMetadata.Empty;
        }

        var properties = propertyAccess.Split('.');
        var value      = properties.Aggregate(target, GetValueByKey);
        return new(Name: properties.Last(), Value: value, Type: value?.GetType() ?? typeof(object));
    }
    
    private static object GetValueByKey(object target, string key)
        => target switch
        {
            _ when string.IsNullOrWhiteSpace(key)
                => default,

            IDictionary dictionary
                => dictionary.Contains(key)
                    ? dictionary[key]
                    : default,

            not null
                => GetValueOfMemberInfo(target, key),

            _ => default
        };

    private static object GetValueOfMemberInfo(object target, string memberName)
    {
        var type = target.GetType();

        if (type.GetProperty(memberName) is { } property)
        {
            return property.GetValue(target);
        }

        if (type.GetField(memberName) is { } field)
        {
            return field.GetValue(field);
        }

        return default;
    }
}