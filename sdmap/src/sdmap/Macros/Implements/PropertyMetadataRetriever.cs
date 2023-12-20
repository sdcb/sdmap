using System;
using System.Collections;
using System.Linq;

namespace sdmap.Macros.Implements;

using static PropertyMetadata;

internal readonly record struct PropertyMetadata(string Name, object Value, bool Exists = true)
{
    public Type Type => Value?.GetType() ?? typeof(object);

    public static PropertyMetadata DoesNotExist => new(string.Empty, default, false);

    public static PropertyMetadata Root(object value) => new(string.Empty, value);
}

internal static class PropertyMetadataRetriever
{
    public static PropertyMetadata Get(object target, string propertyAccess)
    {
        if (string.IsNullOrWhiteSpace(propertyAccess))
        {
            return DoesNotExist;
        }

        return propertyAccess
            .Split('.')
            .Aggregate(Root(target), (metadata, next) => GetByKey(metadata.Value, next));
    }

    private static PropertyMetadata GetByKey(object target, string key)
        => target switch
        {
            _ when string.IsNullOrWhiteSpace(key)
                => DoesNotExist,

            IDictionary dictionary
                => dictionary.Contains(key)
                    ? new(key, dictionary[key])
                    : DoesNotExist,

            not null
                => GetByMemberName(target, key),

            _ => DoesNotExist
        };

    private static PropertyMetadata GetByMemberName(object target, string memberName)
    {
        var type = target.GetType();

        if (type.GetProperty(memberName) is { } property)
        {
            return new(memberName, property.GetValue(target));
        }

        if (type.GetField(memberName) is { } field)
        {
            return  new(memberName, field.GetValue(target));
        }

        return DoesNotExist;
    }
}