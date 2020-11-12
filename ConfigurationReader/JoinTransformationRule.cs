using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class JoinTransformationRule : TransformationRule
    {
        public string PropertyName => "join";

        public IEnumerable<Configuration> DependentConfigurations(Configuration configuration)
            => configuration[PropertyName].Items;

        public Configuration Apply(Configuration configuration)
            => Configuration.String(string.Join(string.Empty, configuration[PropertyName].Items.Select(item => item.String())));
    }
}
