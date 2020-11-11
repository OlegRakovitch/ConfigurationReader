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
            var configuration = new Configuration("string");
            Assert.Equal("string", configuration);
        }

        [Fact]
        public void CanCreateIntConfiguration()
        {
            var configuration = new Configuration(42);
            Assert.Equal(42, configuration);
        }

        [Fact]
        public void CanCreateBoolConfiguration()
        {
            var configuration = new Configuration(true);
            Assert.True(configuration);
        }

        [Fact]
        public void CanCreateStringArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration("value1"), new Configuration("value2") });
            Assert.Equal(new[] { "value1", "value2" }, configuration);
            Assert.Equal("value1", configuration[0]);
            Assert.Equal("value2", configuration[1]);
        }

        [Fact]
        public void CanCreateIntArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration(42), new Configuration(1337) });
            Assert.Equal(new[] { 42, 1337 }, configuration);
            Assert.Equal(42, configuration[0]);
            Assert.Equal(1337, configuration[1]);
        }

        [Fact]
        public void CanCreateBoolArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration(true), new Configuration(false) });
            Assert.Equal(new[] { true, false }, configuration);
            Assert.True(configuration[0]);
            Assert.False(configuration[1]);
        }

        [Fact]
        public void CanCreateObjectConfiguration()
        {
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "stringKey", new Configuration("value") },
                { "intKey", new Configuration(42) },
                { "boolKey", new Configuration(true) }
            });
            Assert.Equal("value", configuration["stringKey"]);
            Assert.Equal(42, configuration["intKey"]);
            Assert.True(configuration["boolKey"]);
        }

        [Fact]
        public void CanUpdateStringArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration("value") });
            configuration[0] = new Configuration("value2");
            Assert.Equal("value2", configuration[0]);
        }

        [Fact]
        public void CanUpdateIntArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration(42) });
            configuration[0] = new Configuration(1337);
            Assert.Equal(1337, configuration[0]);
        }

        [Fact]
        public void CanUpdateBoolArrayConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration(true) });
            configuration[0] = new Configuration(false);
            Assert.False(configuration[0]);
        }

        [Fact]
        public void CanReferenceAnotherConfiguration()
        {
            var configuration = new Configuration(new[] { new Configuration(true), new Configuration(false) });
            configuration[0] = configuration[1];
            Assert.Equal(configuration[0], configuration[1]);
        }
    }
}
