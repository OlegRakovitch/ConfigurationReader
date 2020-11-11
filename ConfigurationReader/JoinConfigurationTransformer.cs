using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class JoinConfigurationTransformer : ConfigurationTransformerBase
    {
        protected override Configuration Transformation(Configuration configuration)
        {
            if (NeedToTransform(configuration))
            {
                return TransformItems(TransformedItems(ItemsToTransform(configuration)));
            }
            return configuration;
        }

        bool NeedToTransform(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.TryGetValue("operator", out var value) && "join" == value && configuration.Properties.ContainsKey("data");

        IEnumerable<Configuration> ItemsToTransform(Configuration configuration)
            => configuration["data"].Items;

        IEnumerable<Configuration> TransformedItems(IEnumerable<Configuration> items)
            => items.Select(item => Transform(item));

        Configuration TransformItems(IEnumerable<Configuration> items)
            => Configuration.String(string.Join(string.Empty, items.Select(item => item.String())));
    }
}
