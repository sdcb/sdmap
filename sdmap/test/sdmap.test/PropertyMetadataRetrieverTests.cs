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
        var metadata = PropertyMetadataRetriever.Get(target: null, "Prop");

        ShouldBeEmpty(metadata);
    }

    [Theory]
    [InlineData(data: null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NullOrEmptyProp_ReturnsNull(string propertyAccess)
    {
        var target = new ComplexObject { Prop = "Test" };

        var metadata = PropertyMetadataRetriever.Get(target, propertyAccess);

        ShouldBeEmpty(metadata);
    }

    [Fact]
    public void NonExistentProp_ReturnsNull()
    {
        var target = new ComplexObject { Prop = "Test" };

        var metadata = PropertyMetadataRetriever.Get(target, "NonExistent");

        ShouldBeEmpty(metadata);
    }

    [Fact]
    public void ValidProp_ReturnsValue()
    {
        var target = new ComplexObject { Prop = "Test" };

        var metadata = PropertyMetadataRetriever.Get(target, "Prop");

        metadata.Name.Should().Be("Prop");
        metadata.Value.Should().Be("Test");
        metadata.Type.Should().Be(typeof(string));
    }
    
    [Fact]
    public void ValidField_ReturnsValue()
    {
        var target = new ComplexObject { Field = "Test" };

        var metadata = PropertyMetadataRetriever.Get(target, "Field");

        metadata.Name.Should().Be("Field");
        metadata.Value.Should().Be("Test");
        metadata.Type.Should().Be(typeof(string));
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

        var metadata = PropertyMetadataRetriever.Get(target, "Prop.Prop");

        metadata.Name.Should().Be("Prop");
        metadata.Value.Should().Be("NestedTest");
        metadata.Type.Should().Be(typeof(string));
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

        var metadata = PropertyMetadataRetriever.Get(target, "Prop.Field");
        
        metadata.Name.Should().Be("Field");
        metadata.Value.Should().Be("NestedTest");
        metadata.Type.Should().Be(typeof(string));
    }

    [Fact]
    public void Dictionary_ReturnsValue()
    {
        var target = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };

        var metadata = PropertyMetadataRetriever.Get(target, "Key");

        metadata.Name.Should().Be("Key");
        metadata.Value.Should().Be("Value");
        metadata.Type.Should().Be(typeof(string));
    }

    [Fact]
    public void DictionaryNonExistentKey_ReturnsNull()
    {
        var target = new Dictionary<string, string>
        {
            ["Key"] = "Value"
        };

        var metadata = PropertyMetadataRetriever.Get(target, "NonExistentKey");

        ShouldBeEmpty(metadata);
    }

    [Fact]
    public void DictionaryWithNestedDictionary_ReturnsValue()
    {
        var target = new Dictionary<string, object>
        {
            ["Key"] =  new Dictionary<string, string> { ["NestedKey"] = "NestedValue" }
        };

        var metadata = PropertyMetadataRetriever.Get(target, "Key.NestedKey");

        metadata.Name.Should().Be("NestedKey");
        metadata.Value.Should().Be("NestedValue");
        metadata.Type.Should().Be(typeof(string));
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

        var metadata = PropertyMetadataRetriever.Get(target, "Prop.DictionaryKey");

        metadata.Name.Should().Be("DictionaryKey");
        metadata.Value.Should().Be("DictionaryValue");
        metadata.Type.Should().Be(typeof(string));
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

        var metadata = PropertyMetadataRetriever.Get(target, "Prop.NonExistentKey");

        ShouldBeEmpty(metadata);
    }

    [Fact]
    public void DictionaryWithNestedObject_ReturnsValue()
    {
        var target = new Dictionary<string, object>
        {
            ["ObjectKey"] = new ComplexObject { Prop = new ComplexObject() }
        };

        var metadata = PropertyMetadataRetriever.Get(target, "ObjectKey.Prop");

        metadata.Name.Should().Be("Prop");
        metadata.Value.Should().BeEquivalentTo(new ComplexObject());
        metadata.Type.Should().Be(typeof(ComplexObject));
    }

    [Fact]
    public void DictionaryWithNestedObject_NonExistentProperty_ReturnsNull()
    {
        var target = new Dictionary<string, object>
        {
            ["ObjectKey"] =  new ComplexObject { Prop = "NestedObjectValue" }
        };

        var metadata = PropertyMetadataRetriever.Get(target, "ObjectKey.NonExistentProp");

        ShouldBeEmpty(metadata);
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

        var metadata = PropertyMetadataRetriever.Get(target, "Prop.NestedObjectKey.Prop");

        metadata.Name.Should().Be("Prop");
        metadata.Value.Should().Be("NestedObjectValue");
        metadata.Type.Should().Be(typeof(string));
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

        var metadata = PropertyMetadataRetriever.Get(target, "NestedObjectKey.Prop.NestedDictionaryKey");

        metadata.Name.Should().Be("NestedDictionaryKey");
        metadata.Value.Should().Be("NestedDictionaryValue");
        metadata.Type.Should().Be(typeof(string));
    }

    private static void ShouldBeEmpty(PropertyMetadata metadata)
    {
        metadata.Name.Should().BeEmpty();
        metadata.Value.Should().Be(null);
        metadata.Type.Should().Be(typeof(object));
    }

    private sealed record ComplexObject
    {
        public object Prop { get; set; }

        // ReSharper disable once NotAccessedField.Local
        public object Field;
    }
}