using System.Collections.Generic;

namespace ConfigurationReader
{
    public interface TransformationRule
    {
        bool IsApplicable(Configuration configuration);

        IEnumerable<Configuration> AffectedConfigurations(Configuration configuration);

        Configuration Apply(IEnumerable<Configuration> items);
    }
}
