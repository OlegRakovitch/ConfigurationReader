using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class ConfigurationTransformer
    {
        readonly TransformationRule[] Rules;

        public ConfigurationTransformer(TransformationRule[] rules)
            => Rules = rules;

        public Configuration Transform(Configuration configuration)
        {
            if (configuration.Items != null)
            {
                for (int i = 0; i < configuration.Items.Length; i++)
                {
                    configuration[i] = Transform(configuration[i]);
                }
            }

            if (configuration.Properties != null)
            {
                foreach (var key in configuration.Properties.Keys)
                {
                    configuration[key] = Transform(configuration[key]);
                }
            }

            foreach (var rule in Rules)
            {
                configuration = Transform(configuration, rule);
            }

            return configuration;
        }

        Configuration Transform(Configuration configuration, TransformationRule rule)
        {
            if (!rule.IsApplicable(configuration)) return configuration;
            
            var transformedConfigurations = Transform(rule.AffectedConfigurations(configuration));
            return rule.Apply(transformedConfigurations);
        }

        IEnumerable<Configuration> Transform(IEnumerable<Configuration> items)
            => items.Select(item => Transform(item));
    }
}
