using Xunit;

namespace ConfigurationReader.Tests
{
    public class JoinTransformationRuleTest
    {
        [Fact]
        public void ReplacesConfigurationWithJoinedConfiguration()
        {
            var transformer = new ConfigurationTransformer(new TransformationRule[]
            {
                new JoinTransformationRule()
            });
            var configuration = Configuration.Dictionary(new ()
            {
                {
                    "join",
                    Configuration.Array(new[]
                    {
                        Configuration.FromValue("value1"),
                        Configuration.FromValue("value2"),
                        Configuration.FromValue("value3")
                    })
                }
            });
            var transformed = transformer.Transform(configuration);
            Assert.Equal("value1value2value3", transformed.Value);
        }
    }
}
