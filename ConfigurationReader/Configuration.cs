using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class Configuration
    {
        readonly object Value;
        Configuration Parent;
        public readonly Configuration[] Items;
        public readonly Dictionary<string, Configuration> Properties;

        public Configuration Root
        {
            get
            {
                var node = this;
                while (node.Parent != null) node = node.Parent;
                return node;
            }
        }

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
            Items = items;
            foreach(var configuration in items)
            {
                configuration.Parent = this;
            }
        }

        public static Configuration Dictionary(Dictionary<string, Configuration> properties)
            => new Configuration(properties);

        protected Configuration(Dictionary<string, Configuration> properties)
        {
            Properties = properties;
            foreach(var configuration in properties.Values)
            {
                configuration.Parent = this;
            }
        }

        public Configuration this[string key]
        {
            get
            {
                return Properties[key];
            }
            set
            {
                Properties[key] = value;
                if (value.Parent == null)
                    value.Parent = this;
            }
        }

        public Configuration this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
                if (value.Parent == null)
                    value.Parent = this;
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
            => configuration.Items.Select(to).ToArray();

        public string String()
            => (string)Value;

        public int Int()
            => (int)Value;

        public bool Bool()
            => (bool)Value;
    }
}
