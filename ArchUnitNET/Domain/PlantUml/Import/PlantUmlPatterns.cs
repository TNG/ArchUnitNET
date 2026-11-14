using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal static class PlantUmlPatterns
    {
        // e.g. '<<Some.Namespace.*>>', can be repeated
        internal const string Stereotypes = @"(?:<<(?<stereotype>[^<>]+)>>\s*)*";

        internal static ISet<string> GetStereotypes(Match match)
        {
            return new HashSet<string>(
                match.Groups["stereotype"].Captures.Cast<Capture>().Select(c => c.Value)
            );
        }
    }

    internal class PlantUmlDependencyMatcher
    {
        // e.g. '-left->' or '--[#green]->'
        private const string ArrowCenter = @"(?:left|right|up|down|\[[^\]]+\])?";

        private static readonly Regex RightArrowPattern = new Regex(
            @"\s-+" + ArrowCenter + @"-*>\s"
        );
        private static readonly Regex LeftArrowPattern = new Regex(
            @"\s<-*" + ArrowCenter + @"-+\s"
        );

        private PlantUmlDependencyMatcher(string origin, string target)
        {
            Origin = origin;
            Target = target;
        }

        internal string Origin { get; }
        internal string Target { get; }

        internal static IEnumerable<PlantUmlDependencyMatcher> TryParse(string line)
        {
            if (RightArrowPattern.IsMatch(line))
            {
                IList<string> parts = ParseParts(line, RightArrowPattern);
                yield return new PlantUmlDependencyMatcher(parts[0], parts[1]);
            }
            if (LeftArrowPattern.IsMatch(line))
            {
                IList<string> parts = ParseParts(line, LeftArrowPattern);
                yield return new PlantUmlDependencyMatcher(parts[1], parts[0]);
            }
        }

        private static IList<string> ParseParts(string line, Regex arrowPattern)
        {
            line = Regex.Replace(line, ":.*", ""); // remove the optional description
            return arrowPattern
                .Replace(line, " ")
                .Split(' ')
                .Select(l => l.Trim())
                .Take(2)
                .ToList();
        }
    }

    internal class PlantUmlComponentMatcher
    {
        private static readonly Regex Pattern = new Regex(
            @"^\s*\[(?<name>[^\[\]]+)\]\s*"
                + PlantUmlPatterns.Stereotypes
                + @"\s*(?:as ""?(?<alias>[^""]+)""?)?\s*$"
        );

        private PlantUmlComponentMatcher(Match match)
        {
            Name = match.Groups["name"].Value;
            Alias = match.Groups["alias"].Success ? match.Groups["alias"].Value : null;
            Stereotypes = PlantUmlPatterns.GetStereotypes(match);
        }

        internal string Name { get; }
        internal string Alias { get; } // null without 'as'
        internal ISet<string> Stereotypes { get; }

        internal static PlantUmlComponentMatcher TryParse(string line)
        {
            Match match = Pattern.Match(line);
            return match.Success ? new PlantUmlComponentMatcher(match) : null;
        }
    }
}
