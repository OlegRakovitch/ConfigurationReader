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
                { "key", Configuration.String("value") },
                { "referenced", ReferenceConfiguration("key")}
            }));
            Assert.Equal("value", transformed["referenced"]);
        }

        [Fact]
        public void CreatesReferenceToAnotherArrayConfiguration()
        {
            var transformed = Transform(Configuration.Array(new[] { Configuration.String("value"), ReferenceConfiguration("[0]") }));
            Assert.Equal("value", transformed[1]);
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
                        { "key2", Configuration.String("value") }
                    })
                },
                { "referenced", ReferenceConfiguration("key.key2")}
            }));
            Assert.Equal("value", transformed["referenced"]);
        }

        [Fact]
        public void CreatesReferenceToNestedArrayConfiguration()
        {
            var transformed = Transform(Configuration.Array(new[]
            {
                Configuration.Array(new[]
                {
                    Configuration.String("value")
                }),
                ReferenceConfiguration("[0][0]")
            }));
            Assert.Equal("value", transformed[1]);
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
                            { "inner", Configuration.String("value") }
                        })
                    })
                },
                { "referenced", ReferenceConfiguration("key[0].inner") }
            }));
            Assert.Equal("value", transformed["referenced"]);
        }

        [Fact]
        public void FollowsSeveralReferencesInConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new ()
            {
                { "key", Configuration.String("value") },
                { "reference1", ReferenceConfiguration("key") },
                { "reference2", ReferenceConfiguration("reference1") }
            }));
            Assert.Equal("value", transformed["reference2"]);
        }

        [Fact]
        public void FollowsSeveralReferencesBackwardsInConfiguration()
        {
            var transformed = Transform(Configuration.Dictionary(new()
            {
                { "key", Configuration.String("value") },
                { "reference1", ReferenceConfiguration("reference2") },
                { "reference2", ReferenceConfiguration("key") }
            }));
            Assert.Equal("value", transformed["reference1"]);
        }

        static Configuration ReferenceConfiguration(string referenceKey)
            => Configuration.Dictionary(new ()
            {
                { "ref", Configuration.String(referenceKey) }
            });

        static Configuration Transform(Configuration configuration)
        {
            var transformer = new ReferenceConfigurationTransformer();
            return transformer.Transform(configuration);
        }
    }
}
