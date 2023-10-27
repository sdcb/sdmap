using System.Collections.Generic;
using sdmap.Macros.Implements;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace sdmap.test;

public class GetPropertyValueTests
{
    [Fact]
    public void NullObject_ReturnsNull()
    {
        var result = DynamicRuntimeMacros.GetPropValue(self: null, "Prop");
        Assert.Null(result);
    }

    [Theory]
    [InlineData(data: null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NullOrEmptyProp_ReturnsNull(string prop)
    {
        var obj    = new ComplexClass { Prop = "Test" };
        var result = DynamicRuntimeMacros.GetPropValue(obj, prop);
        Assert.Null(result);
    }

    [Fact]
    public void ValidProp_ReturnsValue()
    {
        var obj    = new ComplexClass { Prop = "Test" };
        var result = DynamicRuntimeMacros.GetPropValue(obj, "Prop");
        Assert.Equal("Test", result);
    }
    
    [Fact]
    public void ValidField_ReturnsValue()
    {
        var obj    = new ComplexClass { Field = "Test" };
        var result = DynamicRuntimeMacros.GetPropValue(obj, "Field");
        Assert.Equal("Test", result);
    }

    [Fact]
    public void NestedProp_ReturnsValue()
    {
        var obj = new ComplexClass
        {
            Prop = new ComplexClass
            {
                Prop = "NestedTest"
            }
        };
        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "Prop.Prop"
        );
        Assert.Equal("NestedTest", result);
    }

    [Fact]
    public void NestedPropAndField_ReturnsValue()
    {
        var obj = new ComplexClass
        {
            Prop = new ComplexClass
            {
                Field = "NestedTest"
            }
        };
        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "Prop.Field"
        );
        Assert.Equal("NestedTest", result);
    }

    [Fact]
    public void NonExistentProp_ReturnsNull()
    {
        var obj = new ComplexClass { Prop = "Test" };
        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "NonExistent"
        );
        Assert.Null(result);
    }

    [Fact]
    public void Dictionary_ReturnsValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };
        var result = DynamicRuntimeMacros.GetPropValue(dictionary, "Key");
        Assert.Equal("Value", result);
    }

    [Fact]
    public void DictionaryNonExistentKey_ReturnsNull()
    {
        var dictionary = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };
        var result = DynamicRuntimeMacros.GetPropValue(
            dictionary,
            "NonExistentKey"
        );
        Assert.Null(result);
    }

    [Fact]
    public void DictionaryWithNestedDictionary_ReturnsValue()
    {
        var nestedDictionary = new Dictionary<string, string> { ["NestedKey"] = "NestedValue" };
        var dictionary = new Dictionary<string, object>
        {
            ["Key"] = nestedDictionary
        };
        var result = DynamicRuntimeMacros.GetPropValue(
            dictionary,
            "Key.NestedKey"
        );
        Assert.Equal("NestedValue", result);
    }

    [Fact]
    public void ObjectWithNestedDictionary_ReturnsValue()
    {
        var obj = new ComplexClass
        {
            InnerDictionary = new()
            {
                ["DictionaryKey"] = "DictionaryValue"
            }
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "InnerDictionary.DictionaryKey"
        );
        Assert.Equal("DictionaryValue", result);
    }

    [Fact]
    public void ObjectWithNestedDictionary_NonExistentKey_ReturnsNull()
    {
        var obj = new ComplexClass
        {
            InnerDictionary = new()
            {
                ["DictionaryKey"] = "DictionaryValue"
            }
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "InnerDictionary.NonExistentKey"
        );
        Assert.Null(result);
    }

    [Fact]
    public void DictionaryWithNestedObject_ReturnsValue()
    {
        var nestedObj = new ComplexClass { Prop = "NestedObjectValue" };
        var dictionary = new Dictionary<string, object>
        {
            ["ObjectKey"] = nestedObj
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            dictionary,
            "ObjectKey.Prop"
        );
        Assert.Equal("NestedObjectValue", result);
    }

    [Fact]
    public void
        DictionaryWithNestedObject_NonExistentProperty_ReturnsNull()
    {
        var nestedObj = new ComplexClass { Prop = "NestedObjectValue" };
        var dictionary = new Dictionary<string, object>
        {
            ["ObjectKey"] = nestedObj
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            dictionary,
            "ObjectKey.NonExistentProp"
        );
        Assert.Null(result);
    }

    [Fact]
    public void ObjectWithNestedDictionaryAndObject_ReturnsValue()
    {
        var nestedObj = new ComplexClass { Prop = "NestedObjectValue" };
        var obj = new ComplexClass
        {
            InnerDictionary = new()
            {
                ["NestedObjectKey"] = nestedObj
            }
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            obj,
            "InnerDictionary.NestedObjectKey.Prop"
        );
        Assert.Equal("NestedObjectValue", result);
    }

    [Fact]
    public void DictionaryWithNestedObjectAndDictionary_ReturnsValue()
    {
        var nestedDictionary = new Dictionary<string, string>
        {
            ["NestedDictionaryKey"] = "NestedDictionaryValue"
        };
        var nestedObj = new ComplexClass { Prop = nestedDictionary };
        var dictionary = new Dictionary<string, object>
        {
            ["NestedObjectKey"] = nestedObj
        };

        var result = DynamicRuntimeMacros.GetPropValue(
            dictionary,
            "NestedObjectKey.Prop.NestedDictionaryKey"
        );
        Assert.Equal("NestedDictionaryValue", result);
    }

    private sealed class ComplexClass
    {
        public Dictionary<string, object> InnerDictionary { get; set; }

        public object Prop { get; set; }

        public object Field { get; set; }
    }
}