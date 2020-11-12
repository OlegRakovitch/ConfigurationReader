using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void CanCreateStringConfiguration()
        {
            var configuration = Configuration.String("string");
            Assert.Equal("string", configuration);
        }

        [Fact]
        public void CanCreateIntConfiguration()
        {
            var configuration = Configuration.Int(42);
            Assert.Equal(42, configuration);
        }

        [Fact]
        public void CanCreateBoolConfiguration()
        {
            var configuration = Configuration.Bool(true);
            Assert.True(configuration);
        }

        [Fact]
        public void CanCreateStringArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.String("value1"), Configuration.String("value2") });
            Assert.Equal(new[] { "value1", "value2" }, configuration);
            Assert.Equal("value1", configuration[0]);
            Assert.Equal("value2", configuration[1]);
        }

        [Fact]
        public void CanCreateIntArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.Int(42), Configuration.Int(1337) });
            Assert.Equal(new[] { 42, 1337 }, configuration);
            Assert.Equal(42, configuration[0]);
            Assert.Equal(1337, configuration[1]);
        }

        [Fact]
        public void CanCreateBoolArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.Bool(true), Configuration.Bool(false) });
            Assert.Equal(new[] { true, false }, configuration);
            Assert.True(configuration[0]);
            Assert.False(configuration[1]);
        }

        [Fact]
        public void CanCreateObjectConfiguration()
        {
            var configuration = Configuration.Dictionary(new ()
            {
                { "stringKey", Configuration.String("value") },
                { "intKey", Configuration.Int(42) },
                { "boolKey", Configuration.Bool(true) }
            });
            Assert.Equal("value", configuration["stringKey"]);
            Assert.Equal(42, configuration["intKey"]);
            Assert.True(configuration["boolKey"]);
        }

        [Fact]
        public void CanUpdateStringArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.String("value") });
            configuration[0] = Configuration.String("value2");
            Assert.Equal("value2", configuration[0]);
        }

        [Fact]
        public void CanUpdateIntArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.Int(42) });
            configuration[0] = Configuration.Int(1337);
            Assert.Equal(1337, configuration[0]);
        }

        [Fact]
        public void CanUpdateBoolArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.Bool(true) });
            configuration[0] = Configuration.Bool(false);
            Assert.False(configuration[0]);
        }

        [Fact]
        public void CanReferenceAnotherConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.Bool(true), Configuration.Bool(false) });
            configuration[0] = configuration[1];
            Assert.Equal(configuration[0], configuration[1]);
        }

        [Fact]
        public void ReturnsNoKeysIfConfigurationIsEmpty()
        {
            var configuration = Configuration.Dictionary(new() { });
            Assert.Equal(Array.Empty<string>(), configuration.Keys);
        }

        [Fact]
        public void CanIterateConfigurationKeys()
        {
            var configuration = Configuration.Dictionary(new()
            {
                { "key1", Configuration.String("value1") },
                { "key2", Configuration.String("value2") }
            });
            Assert.Equal(new[] { "key1", "key2" }, configuration.Keys);
        }

        [Fact]
        public void ReturnsNoIndicesIfConfigurationIsEmpty()
        {
            var configuration = Configuration.Array(Array.Empty<Configuration>());
            Assert.Equal(Array.Empty<int>(), configuration.Indices);
        }

        [Fact]
        public void CanIterateConfigurationIndices()
        {
            var configuration = Configuration.Array(new[]
            {
                Configuration.String("value1"),
                Configuration.String("value2")
            });
            Assert.Equal(new[] { 0, 1 }, configuration.Indices);
        }

        [Fact]
        public void ReturnsEmptyArrayIfNoConfigurationItems()
        {
            var configuration = Configuration.String("value");
            Assert.Equal(Array.Empty<Configuration>(), configuration.Items);
        }

        [Fact]
        public void ReturnsConfigurationItems()
        {
            var configuration = Configuration.Array(new[]
            {
                Configuration.String("value1"),
                Configuration.String("value2")
            });
            Assert.Equal(new[] { "value1", "value2" }, configuration.Items.Select(item => item.String()));
        }

        [Fact]
        public void ReturnsEmptyArrayIfNoConfigurationProperties()
        {
            var configuration = Configuration.String("value");
            Assert.Equal(Array.Empty<Configuration>(), configuration.Properties);
        }

        [Fact]
        public void ReturnsConfigurationProperties()
        {
            var configuration = Configuration.Dictionary(new()
            {
                { "key1", Configuration.String("value1") },
                { "key2", Configuration.String("value2") }
            });
            Assert.Equal(new[] { "value1", "value2" }, configuration.Properties.Select(item => item.String()));
        }

        [Fact]
        public void ReturnsNullAsParentConfigurationForSimpleConfiguration()
        {
            var configuration = Configuration.String("value");
            Assert.Null(configuration.Parent);
        }

        [Fact]
        public void ReturnsParentConfigurationForDictionaryItem()
        {
            var inner = Configuration.String("value");
            var configuration = Configuration.Dictionary(new()
            {
                { "key", inner }
            });
            Assert.Equal(configuration, inner.Parent);
        }

        [Fact]
        public void ReturnsParentConfigurationForArrayItem()
        {
            var inner = Configuration.String("value");
            var configuration = Configuration.Array(new[] { inner });
            Assert.Equal(configuration, inner.Parent);
        }
    }
}
