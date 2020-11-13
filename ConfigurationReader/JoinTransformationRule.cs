using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class JoinTransformationRule : TransformationRule
    {
        public string PropertyName => "join";

        public IEnumerable<Configuration> DependentConfigurations(Configuration configuration)
            => configuration[PropertyName].Children;

        public Configuration Apply(Configuration configuration)
            => Configuration.FromValue(string.Join(string.Empty, configuration[PropertyName].Children.Select(item => item.Value.String())));
    }
}
