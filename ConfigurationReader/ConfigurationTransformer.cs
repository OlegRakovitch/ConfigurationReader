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
            foreach (var rule in Rules)
            {
                configuration = Transform(configuration, rule);
            }

            foreach (var key in configuration.Keys)
            {
                configuration[key] = Transform(configuration[key]);
            }

            foreach (var index in configuration.Indices)
            {
                configuration[index] = Transform(configuration[index]);
            }

            return configuration;
        }

        Configuration Transform(Configuration configuration, TransformationRule rule)
        {
            if (!IsApplicable(rule, configuration)) return configuration;

            var transformedConfigurations = new List<Configuration>();

            foreach (var dependentConfiguration in rule.DependentConfigurations(configuration))
            {
                var parent = dependentConfiguration.Parent;
                if (parent.Properties.Contains(dependentConfiguration))
                {
                    var transformed = Transform(dependentConfiguration);
                    var keys = parent.Keys.Where(k => parent[k] == dependentConfiguration);
                    foreach(var key in keys)
                        parent[key] = transformed;
                    transformedConfigurations.Add(transformed);
                }
                if (parent.Items.Contains(dependentConfiguration))
                {
                    var transformed = Transform(dependentConfiguration);
                    var indices = parent.Indices.Where(i => parent[i] == dependentConfiguration);
                    foreach(var index in indices)
                        parent[index] = transformed;
                    transformedConfigurations.Add(transformed);
                }
            }
            return rule.Apply(configuration);
        }

        static bool IsApplicable(TransformationRule rule, Configuration configuration)
            => configuration.Keys.Count() == 1
            && configuration.Keys.Single() == rule.PropertyName;

        IEnumerable<Configuration> Transform(IEnumerable<Configuration> items)
            => items.Select(item => Transform(item));
    }
}
