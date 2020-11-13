using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public record ConfigurationKey
    {
        readonly object Value;
        readonly bool IsStringKey;

        public static ConfigurationKey Index(int index)
            => new ConfigurationKey(index);

        public ConfigurationKey(int index)
            => Value = index;

        public static ConfigurationKey Key(string key)
            => new ConfigurationKey(key);

        public ConfigurationKey(string key)
            => (Value, IsStringKey) = (key, true);

        public static implicit operator string(ConfigurationKey key)
            => key.String();

        public static implicit operator int(ConfigurationKey key)
            => key.Int();

        public static implicit operator ConfigurationKey(string key)
            => Key(key);

        public static implicit operator ConfigurationKey(int index)
            => Index(index);

        public string String()
            => (string)Value;

        public int Int()
            => (int)Value;

        public bool IsString()
            => IsStringKey;

        public bool IsInt()
            => !IsStringKey;

        public override string ToString()
            => $"{nameof(ConfigurationKey)} {Value}";
    }

    public class Configuration
    {
        Configuration ParentConfiguration;
        readonly object Value;

        readonly Dictionary<ConfigurationKey, Configuration> ChildrenConfigurations = new Dictionary<ConfigurationKey, Configuration>();

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

        public IEnumerable<Configuration> Properties => Keys.Select(key => ChildrenConfigurations[ConfigurationKey.Key(key)]);

        public IEnumerable<Configuration> Items => Indices.Select(index => ChildrenConfigurations[ConfigurationKey.Index(index)]);

        public IEnumerable<string> Keys => ChildrenConfigurations.Keys.Where(key => key.IsString()).Select(key => key.String());

        public IEnumerable<int> Indices => ChildrenConfigurations.Keys.Where(key => key.IsInt()).Select(key => key.Int());

        public static Configuration String(string value)
            => new Configuration(value);

        protected Configuration(string value)
            => Value = value;

        public static Configuration Int(int value)
            => new Configuration(value);

        protected Configuration(int value)
            => Value = value;

        public static Configuration Bool(bool value)
            => new Configuration(value);

        protected Configuration(bool value)
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

        public static implicit operator string(Configuration configuration)
            => configuration.String();

        public static implicit operator int(Configuration configuration)
            => configuration.Int();
        
        public static implicit operator bool(Configuration configuration)
            => configuration.Bool();

        public static implicit operator string[](Configuration configuration)
            => ToArray(configuration, c => c.String());

        public static implicit operator int[](Configuration configuration)
            => ToArray(configuration, c => c.Int());

        public static implicit operator bool[](Configuration configuration)
            => ToArray(configuration, c => c.Bool());

        static T[] ToArray<T>(Configuration configuration, Func<Configuration, T> to)
            => configuration.ChildrenConfigurations.Values.Select(to).ToArray();

        public string String()
            => (string)Value;

        public int Int()
            => (int)Value;

        public bool Bool()
            => (bool)Value;
    }
}
