using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class Configuration
    {
        Configuration ParentConfiguration;
        readonly Dictionary<ConfigurationKey, Configuration> ChildrenConfigurations = new Dictionary<ConfigurationKey, Configuration>();

        public readonly ConfigurationValue Value;

        public Configuration Root
        {
            get
            {
                var node = this;
                while (node.ParentConfiguration != null) node = node.ParentConfiguration;
                return node;
            }
        }

        public Configuration Parent => ParentConfiguration;

        public IEnumerable<Configuration> Children => ChildrenConfigurations.Values;

        public IEnumerable<ConfigurationKey> Keys => ChildrenConfigurations.Keys;

        public static Configuration FromValue(string value)
            => new Configuration(ConfigurationValue.String(value));

        public static Configuration FromValue(int value)
            => new Configuration(ConfigurationValue.Int(value));

        public static Configuration FromValue(bool value)
            => new Configuration(ConfigurationValue.Bool(value));

        Configuration(ConfigurationValue value)
            => Value = value;

        public static Configuration Array(Configuration[] items)
            => new Configuration(items);

        Configuration(Configuration[] items)
            : this(Enumerable.Range(0, items.Length).ToDictionary(index => ConfigurationKey.Index(index), index => items[index])) { }

        public static Configuration Dictionary(Dictionary<string, Configuration> properties)
            => new Configuration(properties);

        Configuration(Dictionary<string, Configuration> properties)
            : this(properties.Keys.ToDictionary(key => ConfigurationKey.Key(key), key => properties[key])) { }

        Configuration(Dictionary<ConfigurationKey, Configuration> configurations)
        {
            ChildrenConfigurations = configurations;
            foreach (var configuration in configurations.Values)
                configuration.ParentConfiguration = this;
        }

        public Configuration this[ConfigurationKey key]
        {
            get
            {
                return ChildrenConfigurations[key];
            }
            set
            {
                ChildrenConfigurations[key] = value;
                if (value.ParentConfiguration == null)
                    value.ParentConfiguration = this;
            }
        }
    }
}
