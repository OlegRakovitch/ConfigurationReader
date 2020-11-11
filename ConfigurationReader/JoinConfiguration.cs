using System.Linq;

namespace ConfigurationReader
{
    public class JoinConfiguration : Configuration
    {
        public JoinConfiguration(Configuration configuration) : base(Join(configuration)) { }

        static string Join(Configuration configuration)
            => string.Join(string.Empty, configuration.Items.Select<Configuration, string>(c => c));
    }
}
