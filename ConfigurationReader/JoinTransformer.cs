namespace ConfigurationReader
{
    public class JoinConfigurationTransformer
    {
        public Configuration Transform(Configuration configuration)
        {
            if (configuration.Items != null)
            {
                for (int i = 0; i < configuration.Items.Length; i++)
                {
                    configuration.Items[i] = Transform(configuration.Items[i]);
                }
            }

            if (configuration.Properties != null)
            {
                foreach (var key in configuration.Properties.Keys)
                {
                    configuration.Properties[key] = Transform(configuration.Properties[key]);
                }

                if (configuration.Properties.TryGetValue("operator", out var value) && "join" == value && configuration.Properties.TryGetValue("data", out var data))
                {
                    return new JoinConfiguration(data);
                }
            }

            return configuration;
        }
    }
}
