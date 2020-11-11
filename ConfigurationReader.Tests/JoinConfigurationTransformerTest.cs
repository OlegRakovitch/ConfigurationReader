using Xunit;

namespace ConfigurationReader.Tests
{
    public class JoinConfigurationTransformerTest
    {
        [Fact]
        public void DoesNotReplaceConfigurationIfThereIsNoDataSpecified()
        {
            var configuration = Configuration.Dictionary(new ()
            {
                { "operator", Configuration.String("join") }
            });
            var transformed = Transform(configuration);
            Assert.Equal("join", transformed["operator"]);
        }

        [Fact]
        public void DoesNotReplaceConfigurationIfThereIsNoOperatorSpecified()
        {
            var configuration = Configuration.Dictionary(new ()
            {
                {
                    "data",
                    Configuration.Array(new[]
                    {
                        Configuration.String("value1"),
                        Configuration.String("value2")
                    })
                }
            });
            var transformed = Transform(configuration);
            Assert.Equal("value1", transformed["data"][0]);
            Assert.Equal("value2", transformed["data"][1]);
        }

        [Fact]
        public void ReplacesOrdinaryConfigurationWithJoinConfiguration()
        {
            var configuration = Configuration.Dictionary(new ()
            {
                { "operator", Configuration.String("join") },
                { "data", Configuration.Array(new[] { Configuration.String("value1"), Configuration.String("value2") }) }
            });
            var transformed = Transform(configuration);
            Assert.Equal("value1value2", transformed);
        }

        [Fact]
        public void ReplacesOrdinaryNestedConfigurationWithJoinConfiguration()
        {
            var innerConfiguration = Configuration.Dictionary(new ()
            {
                { "operator", Configuration.String("join") },
                { "data", Configuration.Array(new[] { Configuration.String("value2"), Configuration.String("value3") }) }
            });
            var configuration = Configuration.Dictionary(new ()
            {
                { "operator", Configuration.String("join") },
                { "data", Configuration.Array(new[] { Configuration.String("value1"), innerConfiguration }) }
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
