using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader
{
    public class Configuration
    {
        readonly object Value;
        readonly Configuration[] Items;
        readonly Dictionary<string, Configuration> Properties;

        public Configuration(object value)
            => Value = value;

        public Configuration(Configuration[] items)
            => Items = items;

        public Configuration(Dictionary<string, Configuration> properties)
            => Properties = properties;

        public Configuration this[string key]
        {
            get
            {
                return Properties[key];
            }
        }

        public Configuration this[int index] => Items[index];

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

        string String()
            => (string)Value;

        int Int()
            => (int)Value;

        bool Bool()
            => (bool)Value;
    }
}
