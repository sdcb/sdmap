using System.Collections.Generic;
using FluentAssertions;
using sdmap.Macros.Implements;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace sdmap.test;

public class PropertyMetadataRetrieverTests
{
    [Fact]
    public void NullObject_ReturnsNull()
    {
        var (name, value, type) = PropertyMetadataRetriever.Get(target: null, "Prop");

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Theory]
    [InlineData(data: null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NullOrEmptyProp_ReturnsNull(string propertyAccess)
    {
        var target = new ComplexObject { Prop = "Test" };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, propertyAccess);

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Fact]
    public void NonExistentProp_ReturnsNull()
    {
        var target = new ComplexObject { Prop = "Test" };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "NonExistent");

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Fact]
    public void ValidProp_ReturnsValue()
    {
        var target = new ComplexObject { Prop = "Test" };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Prop");

        name.Should().Be("Prop");
        value.Should().Be("Test");
        type.Should().Be(typeof(string));
    }
    
    [Fact]
    public void ValidField_ReturnsValue()
    {
        var target = new ComplexObject { Field = "Test" };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Field");

        name.Should().Be("Field");
        value.Should().Be("Test");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void NestedProp_ReturnsValue()
    {
        var target = new ComplexObject
        {
            Prop = new ComplexObject
            {
                Prop = "NestedTest"
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Prop.Prop");

        name.Should().Be("Prop");
        value.Should().Be("NestedTest");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void NestedPropAndField_ReturnsValue()
    {
        var target = new ComplexObject
        {
            Prop = new ComplexObject
            {
                Field = "NestedTest"
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Prop.Field");
        
        name.Should().Be("Field");
        value.Should().Be("NestedTest");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void Dictionary_ReturnsValue()
    {
        var target = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Key");

        name.Should().Be("Key");
        value.Should().Be("Value");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void DictionaryNonExistentKey_ReturnsNull()
    {
        var target = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "NonExistentKey");

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Fact]
    public void DictionaryWithNestedDictionary_ReturnsValue()
    {
        var target = new Dictionary<string, object>
        {
            ["Key"] =  new Dictionary<string, string> { ["NestedKey"] = "NestedValue" }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Key.NestedKey");

        name.Should().Be("NestedKey");
        value.Should().Be("NestedValue");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void ObjectWithNestedDictionary_ReturnsValue()
    {
        var target = new ComplexObject
        {
            Prop = new Dictionary<string, object>
            {
                ["DictionaryKey"] = "DictionaryValue"
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Prop.DictionaryKey");

        name.Should().Be("DictionaryKey");
        value.Should().Be("DictionaryValue");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void ObjectWithNestedDictionary_NonExistentKey_ReturnsNull()
    {
        var target = new ComplexObject
        {
            Prop = new Dictionary<string, object>
            {
                ["DictionaryKey"] = "DictionaryValue"
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "Prop.NonExistentKey");

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Fact]
    public void DictionaryWithNestedObject_ReturnsValue()
    {
        var target = new Dictionary<string, object>
        {
            ["ObjectKey"] = new ComplexObject { Prop = new ComplexObject() }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "ObjectKey.Prop");

        name.Should().Be("Prop");
        value.Should().BeEquivalentTo(new ComplexObject());
        type.Should().Be(typeof(ComplexObject));
    }

    [Fact]
    public void DictionaryWithNestedObject_NonExistentProperty_ReturnsNull()
    {
        var target = new Dictionary<string, object>
        {
            ["ObjectKey"] =  new ComplexObject { Prop = "NestedObjectValue" }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "ObjectKey.NonExistentProp");

        name.Should().BeEmpty();
        value.Should().Be(null);
        type.Should().Be(typeof(object));
    }

    [Fact]
    public void ObjectWithNestedDictionaryAndObject_ReturnsValue()
    {
        var target = new ComplexObject
        {
            Prop = new Dictionary<string, object>
            {
                ["NestedObjectKey"] = new ComplexObject { Prop = "NestedObjectValue" }
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(
            target,
            "Prop.NestedObjectKey.Prop"
        );

        name.Should().Be("Prop");
        value.Should().Be("NestedObjectValue");
        type.Should().Be(typeof(string));
    }

    [Fact]
    public void DictionaryWithNestedObjectAndDictionary_ReturnsValue()
    {
        var target = new Dictionary<string, object>
        {
            ["NestedObjectKey"] = new ComplexObject
            {
                Prop = new Dictionary<string, string>
                {
                    ["NestedDictionaryKey"] = "NestedDictionaryValue"
                }
            }
        };

        var (name, value, type) = PropertyMetadataRetriever.Get(target, "NestedObjectKey.Prop.NestedDictionaryKey");

        name.Should().Be("NestedDictionaryKey");
        value.Should().Be("NestedDictionaryValue");
        type.Should().Be(typeof(string));
    }

    private sealed record ComplexObject
    {
        public object Prop { get; set; }

        // ReSharper disable once NotAccessedField.Local
        public object Field;
    }
}