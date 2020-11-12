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
        public string PropertyName => "trim";

        public IEnumerable<Configuration> DependentConfigurations(Configuration configuration)
        {
            yield return configuration[PropertyName];
        }

        public Configuration Apply(Configuration configuration)
            => Configuration.String(configuration[PropertyName].String().Trim());
    }

    public class UppercaseTransformationRule : TransformationRule
    {
        public string PropertyName => "uppercase";

        public IEnumerable<Configuration> DependentConfigurations(Configuration configuration)
        {
            yield return configuration[PropertyName];
        }

        public Configuration Apply(Configuration configuration)
            => Configuration.String(configuration[PropertyName].String().ToUpper());
    }
}
