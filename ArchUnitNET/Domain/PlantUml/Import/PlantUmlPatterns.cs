using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal static class PlantUmlPatterns
    {
        // e.g. '<<Some.Namespace.*>>', can be repeated
        internal const string Stereotypes = @"(?:<<(?<stereotype>[^<>]+)>>\s*)*";

        // an optional trailing color, e.g. ' #palegreen' or ' #red;line:blue',
        // see https://plantuml.com/component-diagram#individual-colors
        internal const string Color = @"(?:\s+#[^\s{""]+)?";

        internal static ISet<string> GetStereotypes(Match match)
        {
            return new HashSet<string>(
                match.Groups["stereotype"].Captures.Cast<Capture>().Select(c => c.Value)
            );
        }
    }

    internal class PlantUmlDependencyMatcher
    {
        // '[Some Name]', or a name or alias without brackets and whitespace, e.g. 'some-alias'
        private const string LeftEndpoint = @"(?:\[(?<left>[^\[\]]+)\]|(?<left>[^\s\[\]<>"":]+?))";
        private const string RightEndpoint =
            @"(?:\[(?<right>[^\[\]]+)\]|(?<right>[^\s\[\]<>"":]+?))";

        // a line style, e.g. '[#green]' or '[#red,dashed]'
        private const string Style = @"(?:\[[^\[\]]*\])?";

        // a direction in full or abbreviated to its first one or two characters,
        // see https://plantuml.com/component-diagram#changing-arrows-direction
        private const string Direction = @"(?:left|right|down|up|le|ri|do|l|r|u|d)?";

        private const string Center = Style + Direction + Style;

        // the line of an arrow, e.g. '--', '-up-' or '-[#red]-'
        private const string Line = "(?:-+" + Center + "-*|-*" + Center + "-+)";

        // e.g. '-->', '<--' or '<-->'
        private const string Arrow =
            "(?:(?<leftHead><)" + Line + "(?<rightHead>>)?|" + Line + "(?<rightHead>>))";

        private static readonly Regex Pattern = new Regex(
            @"^\s*" + LeftEndpoint + @"\s*" + Arrow + @"\s*" + RightEndpoint + @"\s*(?::.*)?$" // the optional description, e.g. ': uses'
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
            Match match = Pattern.Match(line);
            if (!match.Success)
            {
                yield break;
            }
            string left = match.Groups["left"].Value;
            string right = match.Groups["right"].Value;
            if (match.Groups["rightHead"].Success)
            {
                yield return new PlantUmlDependencyMatcher(left, right);
            }
            if (match.Groups["leftHead"].Success)
            {
                yield return new PlantUmlDependencyMatcher(right, left);
            }
        }
    }

    internal class PlantUmlComponentMatcher
    {
        private static readonly Regex Pattern = new Regex(
            @"^\s*\[(?<name>[^\[\]]+)\]\s*"
                + PlantUmlPatterns.Stereotypes
                + @"\s*(?:as ""?(?<alias>[^""]+?)""?)?"
                + PlantUmlPatterns.Color
                + @"\s*$"
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
