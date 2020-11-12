using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class Configuration
    {
        Configuration ParentConfiguration;
        readonly object Value;
        readonly Configuration[] ConfigurationItems;
        readonly Dictionary<string, Configuration> ConfigurationProperties;

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

        public IEnumerable<Configuration> Properties => ConfigurationProperties == null ? Enumerable.Empty<Configuration>() : ConfigurationProperties.Values;

        public IEnumerable<Configuration> Items => ConfigurationItems == null ? Enumerable.Empty<Configuration>() : ConfigurationItems;

        public IEnumerable<string> Keys => ConfigurationProperties == null ? Enumerable.Empty<string>() : ConfigurationProperties.Keys;

        public IEnumerable<int> Indices => ConfigurationItems == null ? Enumerable.Empty<int>() : Enumerable.Range(0, ConfigurationItems.Length);


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

        protected Configuration(Configuration[] items)
        {
            ConfigurationItems = items;
            foreach(var configuration in items)
            {
                configuration.ParentConfiguration = this;
            }
        }

        public static Configuration Dictionary(Dictionary<string, Configuration> properties)
            => new Configuration(properties);

        protected Configuration(Dictionary<string, Configuration> properties)
        {
            ConfigurationProperties = properties;
            foreach(var configuration in properties.Values)
            {
                configuration.ParentConfiguration = this;
            }
        }

        public Configuration this[string key]
        {
            get
            {
                return ConfigurationProperties[key];
            }
            set
            {
                ConfigurationProperties[key] = value;
                if (value.ParentConfiguration == null)
                    value.ParentConfiguration = this;
            }
        }

        public Configuration this[int index]
        {
            get
            {
                return ConfigurationItems[index];
            }
            set
            {
                ConfigurationItems[index] = value;
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
            => configuration.ConfigurationItems.Select(to).ToArray();

        public string String()
            => (string)Value;

        public int Int()
            => (int)Value;

        public bool Bool()
            => (bool)Value;
    }
}
