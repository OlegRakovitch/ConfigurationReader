using Newtonsoft.Json;
using Xunit;

namespace ConfigurationReader.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void CanTransformComplexConfiguration()
        {
            var transformer = new ConfigurationTransformer(new TransformationRule[]
            {
                new UppercaseTransformationRule(),
                new TrimTransformationRule(),
                new ReferenceTransformationRule(),
                new JoinTransformationRule()
            });
            var parser = new ConfigurationParser();
            var configuration = new
            {
                files = new[]
                {
                    new
                    {
                        name = " index "
                    }
                },
                contents = new
                {
                    INDEX = "value"
                },
                data = new
                {
                    content = new
                    {
                        @ref = new
                        {
                            @join = new object[]
                            {
                                "contents.",
                                new
                                {
                                    @uppercase = new
                                    {
                                        @trim = new
                                        {
                                            @ref = "data.filename"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    filename = new
                    {
                        @ref = "files[0].name"
                    }
                }
            };
            var parsed = parser.Parse(JsonConvert.SerializeObject(configuration));
            var transformed = transformer.Transform(parsed);
            Assert.Equal("value", transformed["data"]["content"]);
        }
    }
}
