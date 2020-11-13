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

            return configuration;
        }

        Configuration Transform(Configuration configuration, TransformationRule rule)
        {
            if (!IsApplicable(rule, configuration)) return configuration;

            TransformDependentConfigurations(rule.DependentConfigurations(configuration));

            return rule.Apply(configuration);
        }

        void TransformDependentConfigurations(IEnumerable<Configuration> configurations)
        {
            foreach (var dependent in configurations)
            {
                var parent = dependent.Parent;
                var keys = parent.Keys.Where(key => parent[key] == dependent);
                var transformed = Transform(dependent);
                foreach (var key in keys)
                    parent[key] = transformed;
            }
        }

        static bool IsApplicable(TransformationRule rule, Configuration configuration)
            => configuration.Children.Count() == 1
            && configuration.Keys.Single() == rule.PropertyName;
    }
}
