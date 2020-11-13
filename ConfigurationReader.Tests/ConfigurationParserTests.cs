using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class ConfigurationParserTests
    {
        readonly ConfigurationParser Parser;
        
        public ConfigurationParserTests()
        {
            Parser = new ConfigurationParser();
        }

        [Fact]
        public void ThrowsErrorOnNullConfiguration()
        {
            AssertThrowsInvalidConfigurationException(null);
        }

        [Fact]
        public void ThrowsErrorOnEmptyStringConfiguration()
        {
            AssertThrowsInvalidConfigurationException(string.Empty);
        }

        [Fact]
        public void ThrowsErrorOnNonJsonStringConfiguration()
        {
            AssertThrowsInvalidConfigurationException("abc");
        }

        [Fact]
        public void CanReadConfigurationForString()
        {
            var configuration = Parse(@"""value""");
            Assert.Equal("value", configuration.Value);
        }

        [Fact]
        public void CanReadConfigurationForInt()
        {
            var configuration = Parse(42);
            Assert.Equal(42, configuration.Value);
        }

        [Fact]
        public void CanReadConfigurationForBool()
        {
            var configuration = Parse(true);
            Assert.True(configuration.Value);
        }

        [Fact]
        public void CanReadConfigurationForEmptyArray()
        {
            var configuration = Parse(Array.Empty<string>());
            Assert.Empty(configuration.Children);
        }

        [Fact]
        public void ThrowsKeyNotFoundExceptionForNonEmptyStringArray()
        {
            var configuration = Parse(new[] { "value" });
            Assert.Throws<KeyNotFoundException>(() => configuration[1]);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyStringArray()
        {
            var configuration = Parse(new[] { "value" });
            Assert.Equal("value", configuration[0].Value);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyIntArray()
        {
            var configuration = Parse(new[] { 42 });
            Assert.Equal(42, configuration[0].Value);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyBoolArray()
        {
            var configuration = Parse(new[] { true });
            Assert.True(configuration[0].Value);
        }

        [Fact]
        public void CanReadConfigurationForEmptyObject()
        {
            Parse(new { });
        }

        [Fact]
        public void CanReadConfigurationForObjectWithKey()
        {
            Parse(new { key = "value" });
        }

        [Fact]
        public void CanReadConfigurationForObjectWithTwoKeys()
        {
            Parse(new { key = "value", key2 = 42 });
        }

        [Fact]
        public void CanReadNestedConfigurationForObjectWithKey()
        {
            Parse(new { nested = new { key = "value" }, key = 42 });
        }

        [Fact]
        public void ThrowsErrorWhenAccessingNonExistingKey()
        {
            var configuration = Parse(new { key = "value" });
            Assert.Throws<KeyNotFoundException>(() => configuration["non-existing-key"]);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingStringKey()
        {
            var configuration = Parse(new { stringKey = "value" });
            Assert.Equal("value", configuration["stringKey"].Value);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingIntKey()
        {
            var configuration = Parse(new { intKey = 42 });
            Assert.Equal(42, configuration["intKey"].Value);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingArrayKey()
        {
            var configuration = Parse(new { arrayKey = new[] { "value" } });
            Assert.Equal("value", configuration["arrayKey"][0].Value);
        }

        [Fact]
        public void ReturnsComplexNestedHierarchyKey()
        {
            var configuration = Parse(new { arrayKey = new[] { new { nestedKey = new { deepKey = "value" } } } });
            Assert.Equal("value", configuration["arrayKey"][0]["nestedKey"]["deepKey"].Value);
        }

        [Fact]
        public void ReturnsSameObjectWhenAccessedByIndex()
        {
            var configuration = Parse(new[] { new { } });
            Assert.Equal(configuration[0], configuration[0]);
        }

        [Fact]
        public void ReturnsSameObjectWhenAccessedByKey()
        {
            var configuration = Parse(new { key = new { } });
            Assert.Equal(configuration["key"], configuration["key"]);
        }

        void AssertThrowsInvalidConfigurationException(string input)
            => Assert.Throws<InvalidConfigurationException>(() => Parse(input));

        Configuration Parse(object configuration)
            => Parse(JsonConvert.SerializeObject(configuration));

        Configuration Parse(string configuration)
            => Parser.Parse(configuration);
    }
}
