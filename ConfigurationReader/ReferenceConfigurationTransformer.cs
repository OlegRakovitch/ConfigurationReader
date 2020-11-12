using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigurationReader
{
    public class ReferenceTransformationRule : TransformationRule
    {
        const string MatchGroupName = "references";
        readonly static Regex KeysRegex = new Regex(@$"(?<{MatchGroupName}>\[\d+\]|\w+)(?<{MatchGroupName}>\[\d+\]|\.\w+)*");
        readonly static Regex UnnecessaryCharactersRegex = new Regex(@"(\[|\]|\.)*");

        public bool IsApplicable(Configuration configuration)
            => configuration.Properties != null && configuration.Properties.ContainsKey("ref");

        public IEnumerable<Configuration> AffectedConfigurations(Configuration configuration)
        {
            var reference = configuration["ref"];
            var match = KeysRegex.Match(reference);
            var keys = GetKeys(match);
            var node = configuration.Root;
            foreach (var key in keys)
            {
                node = int.TryParse(key, out var index) ? node[index] : node[key];
            }
            yield return node;
        }

        public Configuration Apply(IEnumerable<Configuration> items)
            => items.Single();

        static IEnumerable<string> GetKeys(Match match)
            => match
                .Groups[MatchGroupName]
                .Captures
                .Select(capture => RemoveUnnecessaryCharacters(capture.Value));

        static string RemoveUnnecessaryCharacters(string input)
            => UnnecessaryCharactersRegex.Replace(input, string.Empty);
    }
}
