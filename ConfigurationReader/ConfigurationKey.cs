using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
