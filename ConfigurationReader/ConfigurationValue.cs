namespace ConfigurationReader
{
    public class ConfigurationValue
    {
        readonly object Value;

        public static ConfigurationValue String(string value)
            => new ConfigurationValue(value);

        public ConfigurationValue(string value)
            => Value = value;

        public static ConfigurationValue Int(int value)
            => new ConfigurationValue(value);

        public ConfigurationValue(int value)
            => Value = value;

        public static ConfigurationValue Bool(bool value)
            => new ConfigurationValue(value);

        public ConfigurationValue(bool value)
            => Value = value;

        public static implicit operator string(ConfigurationValue value)
            => (string)value.Value;

        public static implicit operator int(ConfigurationValue value)
            => (int)value.Value;

        public static implicit operator bool(ConfigurationValue value)
            => (bool)value.Value;

        public string String()
            => (string)Value;

        public int Int()
            => (int)Value;

        public bool Bool()
            => (bool)Value;
    }
}
