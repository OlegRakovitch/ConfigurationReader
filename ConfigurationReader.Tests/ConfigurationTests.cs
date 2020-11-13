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
            var configuration = Configuration.FromValue("string");
            Assert.Equal("string", configuration.Value);
        }

        [Fact]
        public void CanCreateIntConfiguration()
        {
            var configuration = Configuration.FromValue(42);
            Assert.Equal(42, configuration.Value);
        }

        [Fact]
        public void CanCreateBoolConfiguration()
        {
            var configuration = Configuration.FromValue(true);
            Assert.True(configuration.Value);
        }

        [Fact]
        public void CanCreateStringArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue("value1"), Configuration.FromValue("value2") });
            Assert.Equal("value1", configuration[0].Value);
            Assert.Equal("value2", configuration[1].Value);
        }

        [Fact]
        public void CanCreateIntArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue(42), Configuration.FromValue(1337) });
            Assert.Equal(42, configuration[0].Value);
            Assert.Equal(1337, configuration[1].Value);
        }

        [Fact]
        public void CanCreateBoolArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue(true), Configuration.FromValue(false) });
            Assert.True(configuration[0].Value);
            Assert.False(configuration[1].Value);
        }

        [Fact]
        public void CanCreateObjectConfiguration()
        {
            var configuration = Configuration.Dictionary(new ()
            {
                { "stringKey", Configuration.FromValue("value") },
                { "intKey", Configuration.FromValue(42) },
                { "boolKey", Configuration.FromValue(true) }
            });
            Assert.Equal("value", configuration["stringKey"].Value);
            Assert.Equal(42, configuration["intKey"].Value);
            Assert.True(configuration["boolKey"].Value);
        }

        [Fact]
        public void CanUpdateStringArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue("value") });
            configuration[0] = Configuration.FromValue("value2");
            Assert.Equal("value2", configuration[0].Value);
        }

        [Fact]
        public void CanUpdateIntArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue(42) });
            configuration[0] = Configuration.FromValue(1337);
            Assert.Equal(1337, configuration[0].Value);
        }

        [Fact]
        public void CanUpdateBoolArrayConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue(true) });
            configuration[0] = Configuration.FromValue(false);
            Assert.False(configuration[0].Value);
        }

        [Fact]
        public void CanReferenceAnotherConfiguration()
        {
            var configuration = Configuration.Array(new[] { Configuration.FromValue(true), Configuration.FromValue(false) });
            configuration[0] = configuration[1];
            Assert.Equal(configuration[0], configuration[1]);
        }

        [Fact]
        public void ReturnsNoKeysIfConfigurationIsEmpty()
        {
            var configuration = Configuration.Dictionary(new() { });
            Assert.Equal(Array.Empty<ConfigurationKey>(), configuration.Keys);
        }

        [Fact]
        public void CanIterateConfigurationKeys()
        {
            var configuration = Configuration.Dictionary(new()
            {
                { "key1", Configuration.FromValue("value1") },
                { "key2", Configuration.FromValue("value2") }
            });
            Assert.Equal(new[] { "key1", "key2" }, configuration.Keys.Select(key => key.String()));
        }

        [Fact]
        public void ReturnsEmptyArrayIfNoConfigurationItems()
        {
            var configuration = Configuration.FromValue("value");
            Assert.Equal(Array.Empty<Configuration>(), configuration.Children);
        }

        [Fact]
        public void ReturnsConfigurationItems()
        {
            var configuration = Configuration.Array(new[]
            {
                Configuration.FromValue("value1"),
                Configuration.FromValue("value2")
            });
            Assert.Equal(new[] { "value1", "value2" }, configuration.Children.Select(item => item.Value.String()));
        }

        [Fact]
        public void ReturnsConfigurationProperties()
        {
            var configuration = Configuration.Dictionary(new()
            {
                { "key1", Configuration.FromValue("value1") },
                { "key2", Configuration.FromValue("value2") }
            });
            Assert.Equal(new[] { "value1", "value2" }, configuration.Children.Select(item => item.Value.String()));
        }

        [Fact]
        public void ReturnsNullAsParentConfigurationForSimpleConfiguration()
        {
            var configuration = Configuration.FromValue("value");
            Assert.Null(configuration.Parent);
        }

        [Fact]
        public void ReturnsParentConfigurationForDictionaryItem()
        {
            var inner = Configuration.FromValue("value");
            var configuration = Configuration.Dictionary(new()
            {
                { "key", inner }
            });
            Assert.Equal(configuration, inner.Parent);
        }

        [Fact]
        public void ReturnsParentConfigurationForArrayItem()
        {
            var inner = Configuration.FromValue("value");
            var configuration = Configuration.Array(new[] { inner });
            Assert.Equal(configuration, inner.Parent);
        }
    }
}
