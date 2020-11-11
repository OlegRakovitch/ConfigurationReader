using System.Collections.Generic;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class JoinConfigurationTransformerTest
    {
        [Fact]
        public void DoesNotReplaceConfigurationIfThereIsNoDataSpecified()
        {
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "operator", new Configuration("join") }
            });
            var transformed = Transform(configuration);
            Assert.Equal("join", transformed["operator"]);
        }

        [Fact]
        public void DoesNotReplaceConfigurationIfThereIsNoOperatorSpecified()
        {
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "data", new Configuration(new[] { new Configuration("value1"), new Configuration("value2") }) }
            });
            var transformed = Transform(configuration);
            Assert.Equal("value1", transformed["data"][0]);
            Assert.Equal("value2", transformed["data"][1]);
        }

        [Fact]
        public void ReplacesOrdinaryConfigurationWithJoinConfiguration()
        {
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "operator", new Configuration("join") },
                { "data", new Configuration(new [] { new Configuration("value1"), new Configuration("value2") }) }
            });
            var transformed = Transform(configuration);
            Assert.Equal("value1value2", transformed);
        }

        [Fact]
        public void ReplacesOrdinaryNestedConfigurationWithJoinConfiguration()
        {
            var innerConfiguration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "operator", new Configuration("join") },
                { "data", new Configuration(new[] { new Configuration("value2"), new Configuration("value3")}) }
            });
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "operator", new Configuration("join") },
                { "data", new Configuration(new[] { new Configuration("value1"), innerConfiguration }) }
            });
            var transformed = Transform(configuration);
            Assert.Equal("value1value2value3", transformed);
        }

        static Configuration Transform(Configuration configuration)
        {
            var transformer = new JoinConfigurationTransformer();
            return transformer.Transform(configuration);
        }
    }
}
