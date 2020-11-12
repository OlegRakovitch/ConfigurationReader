using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class ConfigurationTransformerTests
    {
        [Fact]
        public void CanTransformUsingMultipleTransformers()
        {
            var transformer = new ConfigurationTransformer(new TransformationRule[]
            {
                new UppercaseTransformationRule(),
                new TrimTransformationRule()
            });
            var configuration = Configuration.Dictionary(new()
            {
                {
                    "key",
                    Configuration.Dictionary(new()
                    {
                        {
                            "uppercase",
                            Configuration.Dictionary(new()
                            {
                                { "trim", Configuration.String(" value ") }
                            })
                        }
                    })
                }
            });
            var transformed = transformer.Transform(configuration);
            Assert.Equal("VALUE", transformed["key"]);
        }
    }

    public class TrimTransformationRule : TransformationRule
    {
        public bool IsApplicable(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.ContainsKey("trim");

        public IEnumerable<Configuration> AffectedConfigurations(Configuration configuration)
        {
            yield return configuration["trim"];
        }

        public Configuration Apply(IEnumerable<Configuration> items)
            => Configuration.String(items.Single().String().Trim());
    }

    public class UppercaseTransformationRule : TransformationRule
    {
        public bool IsApplicable(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.ContainsKey("uppercase");

        public IEnumerable<Configuration> AffectedConfigurations(Configuration configuration)
        {
            yield return configuration["uppercase"];
        }

        public Configuration Apply(IEnumerable<Configuration> items)
            => Configuration.String(items.Single().String().ToUpper());
    }
}
