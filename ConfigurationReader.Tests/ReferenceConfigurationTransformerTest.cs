using Xunit;

namespace ConfigurationReader.Tests
{
    public class ReferenceConfigurationTransformerTest
    {
        [Fact]
        public void CreatesReferenceToAnotherDictionaryConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new ()
            {
                { "key", Configuration.FromValue("value") },
                { "referenced", ReferenceConfiguration("key")}
            }));
            Assert.Equal("value", transformed["referenced"].Value);
        }

        [Fact]
        public void CreatesReferenceToAnotherArrayConfiguration()
        {
            var transformed = Transform(Configuration.Array(new[] { Configuration.FromValue("value"), ReferenceConfiguration("[0]") }));
            Assert.Equal("value", transformed[1].Value);
        }

        [Fact]
        public void CreatesReferenceToNestedDictionaryConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new ()
            {
                {
                    "key",
                    Configuration.Dictionary(new ()
                    {
                        { "key2", Configuration.FromValue("value") }
                    })
                },
                { "referenced", ReferenceConfiguration("key.key2")}
            }));
            Assert.Equal("value", transformed["referenced"].Value);
        }

        [Fact]
        public void CreatesReferenceToNestedArrayConfiguration()
        {
            var transformed = Transform(Configuration.Array(new[]
            {
                Configuration.Array(new[]
                {
                    Configuration.FromValue("value")
                }),
                ReferenceConfiguration("[0][0]")
            }));
            Assert.Equal("value", transformed[1].Value);
        }

        [Fact]
        public void CreatesReferenceToNestedDictionaryArrayConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new ()
            {
                {
                    "key",
                    Configuration.Array(new []
                    {
                        Configuration.Dictionary(new ()
                        {
                            { "inner", Configuration.FromValue("value") }
                        })
                    })
                },
                { "referenced", ReferenceConfiguration("key[0].inner") }
            }));
            Assert.Equal("value", transformed["referenced"].Value);
        }

        [Fact]
        public void FollowsSeveralReferencesInConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new ()
            {
                { "key", Configuration.FromValue("value") },
                { "reference1", ReferenceConfiguration("key") },
                { "reference2", ReferenceConfiguration("reference1") }
            }));
            Assert.Equal("value", transformed["reference2"].Value);
        }

        [Fact]
        public void FollowsSeveralReferencesBackwardsInConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new()
            {
                { "key", Configuration.FromValue("value") },
                { "reference1", ReferenceConfiguration("reference2") },
                { "reference2", ReferenceConfiguration("key") }
            }));
            Assert.Equal("value", transformed["reference1"].Value);
        }

        static Configuration ReferenceConfiguration(string referenceKey)
            => Configuration.Dictionary(new ()
            {
                { "ref", Configuration.FromValue(referenceKey) }
            });

        static Configuration Transform(Configuration configuration)
        {
            var transformer = new ConfigurationTransformer(new TransformationRule[]
            {
                new ReferenceTransformationRule()
            });
            return transformer.Transform(configuration);
        }
    }
}
