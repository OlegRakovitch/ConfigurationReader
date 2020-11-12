using System.Collections.Generic;

namespace ConfigurationReader
{
    public interface TransformationRule
    {
        string PropertyName { get; }

        IEnumerable<Configuration> DependentConfigurations(Configuration configuration);

        Configuration Apply(Configuration configuration);
    }
}
