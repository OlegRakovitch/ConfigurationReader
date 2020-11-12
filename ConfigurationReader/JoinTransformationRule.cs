using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class JoinTransformationRule : TransformationRule
    {
        public bool IsApplicable(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.ContainsKey("join");

        public IEnumerable<Configuration> AffectedConfigurations(Configuration configuration)
            => configuration.Properties["join"].Items;

        public Configuration Apply(IEnumerable<Configuration> items)
            => Configuration.String(string.Join(string.Empty, items.Select(item => item.String())));
    }
}
