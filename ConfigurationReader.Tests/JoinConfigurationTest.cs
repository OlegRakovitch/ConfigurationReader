using System.Collections.Generic;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class JoinConfigurationTest
    {
        [Fact]
        public void JoinsStringsIntoSingleString()
        {
            var configuration = new JoinConfiguration(new Configuration(new[] { new Configuration("value1"), new Configuration("value2") }));
            Assert.Equal("value1value2", configuration);
        }

        [Fact]
        public void CanBeUsedInsideAnotherConfiguration()
        {
            var configuration = new Configuration(new Dictionary<string, Configuration>()
            {
                { "joined", new JoinConfiguration(new Configuration(new[] { new Configuration("value1"), new Configuration("value2") })) }
            });
            Assert.Equal("value1value2", configuration["joined"]);
        }
    }
}
