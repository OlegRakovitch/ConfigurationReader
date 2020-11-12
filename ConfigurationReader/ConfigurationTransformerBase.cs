namespace ConfigurationReader
{
    public class ConfigurationTransformerBase
    {
        protected Configuration Root;

        public Configuration Transform(Configuration configuration)
        {
            if (Root == null) Root = configuration;

            if (configuration.Items != null)
            {
                for (int i = 0; i < configuration.Items.Length; i++)
                {
                    configuration[i] = Transform(configuration[i]);
                }
            }

            if (configuration.Properties != null)
            {
                foreach (var key in configuration.Properties.Keys)
                {
                    configuration[key] = Transform(configuration[key]);
                }
            }

            return Transformation(configuration);
        }

        protected virtual Configuration Transformation(Configuration configuration) => configuration;
    }
}
