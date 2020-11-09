using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class Reader
    {
        public Configuration Read(string configuration)
        {
            return new Configuration(configuration);
        }
    }

    public class Configuration
    {
        readonly JToken Root;
        readonly Configuration[] Items;
        readonly Dictionary<string, Configuration> Properties;

        public Configuration(string configuration) : this(Parse(configuration)) { }

        Configuration(JToken root)
        {
            Root = root;
            Properties = ReadProperties(root);
            Items = ReadItems(root);
        }

        Configuration[] ReadItems(JToken root)
            => root
                .Children<JToken>()
                .Select(c => CreateConfiguration(c))
                .ToArray();

        Dictionary<string, Configuration> ReadProperties(JToken root) => root switch
        {
            JObject obj => ReadProperties(obj),
            _ => new Dictionary<string, Configuration>()
        };

        Dictionary<string, Configuration> ReadProperties(JObject obj)
            => obj
                .Properties()
                .ToDictionary(p => p.Name, p => CreateConfiguration(p.Value));

        Configuration CreateConfiguration(JToken token)
            => new Configuration(token);

        public Configuration this[string key] => Properties[key];

        public Configuration this[int index] => Items[index];

        static JToken Parse(string configuration)
        {
            try
            {
                return JToken.Parse(configuration);
            }
            catch (Exception)
            {
                throw new InvalidConfigurationException();
            }
        }

        public static implicit operator string(Configuration configuration)
            => To<string>(configuration);

        public static implicit operator int(Configuration configuration)
            => To<int>(configuration);

        public static implicit operator bool(Configuration configuration)
            => To<bool>(configuration);

        static T To<T>(Configuration configuration)
            => configuration.Root.Value<T>();

        public static implicit operator string[](Configuration configuration)
            => ToArray<string>(configuration);

        public static implicit operator int[](Configuration configuration)
            => ToArray<int>(configuration);

        public static implicit operator bool[](Configuration configuration)
            => ToArray<bool>(configuration);

        static T[] ToArray<T>(Configuration configuration)
            => configuration.Root.Children<JToken>().Select(token => token.Value<T>()).ToArray();

    }

    public class InvalidConfigurationException : Exception { }
}
