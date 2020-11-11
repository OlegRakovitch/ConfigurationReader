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
    }
}
