using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class ReaderTests
    {
        readonly Reader Reader;
        
        public ReaderTests()
        {
            Reader = new Reader();
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
            var configuration = Read(@"""value""");
            Assert.Equal("value", configuration);
        }

        [Fact]
        public void CanReadConfigurationForInt()
        {
            var configuration = Read(42);
            Assert.Equal(42, configuration);
        }

        [Fact]
        public void CanReadConfigurationForBool()
        {
            var configuration = Read(true);
            Assert.True(configuration);
        }

        [Fact]
        public void CanReadConfigurationForEmptyStringArray()
        {
            var configuration = Read(Array.Empty<string>());
            Assert.Equal(Array.Empty<string>(), configuration);
        }

        [Fact]
        public void CanReadConfigurationForEmptyIntArray()
        {
            var configuration = Read(Array.Empty<int>());
            Assert.Equal(Array.Empty<int>(), configuration);
        }

        [Fact]
        public void CanReadConfigurationForEmptyBoolArray()
        {
            var configuration = Read(Array.Empty<bool>());
            Assert.Equal(Array.Empty<bool>(), configuration);
        }

        [Fact]
        public void ThrowsIndexOutOfRangeExceptionForNonEmptyStringArray()
        {
            var configuration = Read(new[] { "value" });
            Assert.Throws<IndexOutOfRangeException>(() => configuration[1]);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyStringArray()
        {
            var configuration = Read(new[] { "value" });
            Assert.Equal("value", configuration[0]);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyIntArray()
        {
            var configuration = Read(new[] { 42 });
            Assert.Equal(42, configuration[0]);
        }

        [Fact]
        public void CanReadConfigurationForNonEmptyBoolArray()
        {
            var configuration = Read(new[] { true });
            Assert.True(configuration[0]);
        }

        [Fact]
        public void CanReadConfigurationForEmptyObject()
        {
            Read(new { });
        }

        [Fact]
        public void CanReadConfigurationForObjectWithKey()
        {
            Read(new { key = "value" });
        }

        [Fact]
        public void CanReadConfigurationForObjectWithTwoKeys()
        {
            Read(new { key = "value", key2 = 42 });
        }

        [Fact]
        public void CanReadNestedConfigurationForObjectWithKey()
        {
            Read(new { nested = new { key = "value" }, key = 42 });
        }

        [Fact]
        public void ThrowsErrorWhenAccessingNonExistingKey()
        {
            var configuration = Read(new { key = "value" });
            Assert.Throws<KeyNotFoundException>(() => configuration["non-existing-key"]);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingStringKey()
        {
            var configuration = Read(new { stringKey = "value" });
            Assert.Equal("value", configuration["stringKey"]);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingIntKey()
        {
            var configuration = Read(new { intKey = 42 });
            Assert.Equal(42, configuration["intKey"]);
        }

        [Fact]
        public void ReturnsValueWhenAccessingExistingArrayKey()
        {
            var configuration = Read(new { arrayKey = new[] { "value" } });
            Assert.Equal("value", configuration["arrayKey"][0]);
        }

        [Fact]
        public void ReturnsComplexNestedHierarchyKey()
        {
            var configuration = Read(new { arrayKey = new[] { new { nestedKey = new { deepKey = "value" } } } });
            Assert.Equal("value", configuration["arrayKey"][0]["nestedKey"]["deepKey"]);
        }

        [Fact]
        public void ReturnsSameObjectWhenAccessedByIndex()
        {
            var configuration = Read(new[] { new { } });
            Assert.Equal(configuration[0], configuration[0]);
        }

        [Fact]
        public void ReturnsSameObjectWhenAccessedByKey()
        {
            var configuration = Read(new { key = new { } });
            Assert.Equal(configuration["key"], configuration["key"]);
        }

        void AssertThrowsInvalidConfigurationException(string input)
            => Assert.Throws<InvalidConfigurationException>(() => Read(input));

        Configuration Read(object configuration)
            => Read(JsonConvert.SerializeObject(configuration));

        Configuration Read(string configuration)
            => Reader.Read(configuration);
    }
}
