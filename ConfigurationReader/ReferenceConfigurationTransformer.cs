using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigurationReader
{
    public class ReferenceConfigurationTransformer : ConfigurationTransformerBase
    {
        const string MatchGroupName = "references";
        readonly Regex KeysRegex = new Regex(@$"(?<{MatchGroupName}>\[\d+\]|\w+)(?<{MatchGroupName}>\[\d+\]|\.\w+)*");
        readonly Regex UnnecessaryCharactersRegex = new Regex(@"(\[|\]|\.)*");

        protected override Configuration Transformation(Configuration configuration)
        {
            if (NeedToTransform(configuration))
            {
                return TransformItems(TransformedItems(ItemsToTransform(configuration)));
            }
            return configuration;
        }

        bool NeedToTransform(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.ContainsKey("ref");

        IEnumerable<Configuration> ItemsToTransform(Configuration configuration)
        {
            var reference = configuration.Properties["ref"];
            var match = KeysRegex.Match(reference);
            var keys = GetKeys(match);
            var node = Root;
            foreach (var key in keys)
            {
                node = int.TryParse(key, out var index) ? node[index] : node[key];
            }
            yield return node;
        }

        IEnumerable<Configuration> TransformedItems(IEnumerable<Configuration> items)
            => items.Select(item => Transform(item));

        Configuration TransformItems(IEnumerable<Configuration> items)
            => items.Single();

        IEnumerable<string> GetKeys(Match match)
            => match
                .Groups[MatchGroupName]
                .Captures
                .Select(capture => RemoveUnnecessaryCharacters(capture.Value));

        string RemoveUnnecessaryCharacters(string input)
            => UnnecessaryCharactersRegex.Replace(input, string.Empty);
    }
}
