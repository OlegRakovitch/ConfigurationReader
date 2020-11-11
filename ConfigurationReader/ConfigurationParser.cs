using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReader
{
    public class ConfigurationParser
    {
        public Configuration Parse(string configuration)
            => CreateConfiguration(ParseConfiguration(configuration));

        static JToken ParseConfiguration(string configuration)
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

        Configuration CreateConfiguration(JToken token) => token switch
        {
            JObject obj => Configuration.Dictionary(ReadProperties(obj)),
            JValue value => ValueConfiguration(value),
            JArray array => Configuration.Array(ReadItems(array)),
            _ => throw new InvalidConfigurationException()
        };

        Dictionary<string, Configuration> ReadProperties(JObject obj)
            => obj
                .Properties()
                .ToDictionary(p => p.Name, p => CreateConfiguration(p.Value));

        Configuration[] ReadItems(JArray root)
            => root
                .Select(c => CreateConfiguration(c))
                .ToArray();

        static Configuration ValueConfiguration(JValue value) => value.Type switch
        {
            JTokenType.String => Configuration.String(value.Value<string>()),
            JTokenType.Integer => Configuration.Int(value.Value<int>()),
            JTokenType.Boolean => Configuration.Bool(value.Value<bool>()),
            _ => null
        };
    }
    
    public class InvalidConfigurationException : Exception { }
}
