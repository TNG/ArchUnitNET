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

    // Matches a link between two components, see e.g.
    // https://plantuml.com/class-diagram#relations-between-classes and
    // https://plantuml.com/deployment-diagram#linking-or-arrow
    internal class PlantUmlDependencyMatcher
    {
        // '[Some Name]', or a name or alias without brackets, e.g. 'some-alias' or 'Some.Name'
        private const string PlainName = @"[^\s\[\]<>"":*#+|^@(){}\\~=]+?";
        private const string LeftEndpoint = @"(?:\[(?<left>[^\[\]]+)\]|(?<left>" + PlainName + "))";
        private const string RightEndpoint =
            @"(?:\[(?<right>[^\[\]]+)\]|(?<right>" + PlainName + "))";

        // a multiplicity next to an endpoint, e.g. '"1"' or '"many"'
        private const string Multiplicity = @"(?:""[^""]*""\s*)?";

        // Decorations made of letters or digits must be separated from a name without brackets,
        // so that e.g. 'a--oak' is a link to 'oak' and not an aggregation of 'ak'.
        private const string SeparatedOnTheLeft = @"(?<![^\s\]""])";
        private const string SeparatedOnTheRight = @"(?![^\s\[""])";

        // Heads that point at the component that is depended upon, e.g. '<', '<|' or '^',
        // and composition or aggregation diamonds, which sit at the component that depends
        // on the other. Other decorations, e.g. '#', 'x', '}', '+', '0', '@', ')' or '()',
        // allow no dependency.
        private const string LeftDecoration =
            @"(?:(?<leftArrow><\||<<|<|\^)"
            + @"|(?<leftDiamond>\*)"
            + "|"
            + SeparatedOnTheLeft
            + "(?<leftDiamond>o)"
            + @"|\(\)|[#}+@)]"
            + "|"
            + SeparatedOnTheLeft
            + @"(?:x|0\)?))?";

        // the mirror images of the left decorations, plus '||>', ':|>' and '\\' (a half arrow)
        private const string RightDecoration =
            @"(?:(?<rightArrow>:\|>|\|\|>|\|>|>>|>|\^|\\\\)"
            + @"|(?<rightDiamond>\*)"
            + "|(?<rightDiamond>o)"
            + SeparatedOnTheRight
            + @"|\(\)|\(0|[#{+@(]"
            + "|(?:x|0)"
            + SeparatedOnTheRight
            + ")?";

        // a line style, e.g. '[#green]' or '[#red,dashed]'
        private const string Style = @"\[(?<style>[^\[\]]*)\]";

        // a direction in full or abbreviated to its first one or two characters,
        // see https://plantuml.com/component-diagram#changing-arrows-direction
        private const string Direction = "left|right|down|up|le|ri|do|l|r|u|d";

        // a circle or socket in the middle of a line, e.g. '-0-', '-0)-' or '-(0)-'
        private const string Circle = @"\(0\)|\(0|0\)|0";

        private const string MiddleToken = "(?:" + Style + "|" + Direction + "|" + Circle + ")";

        // plain ('--'), dashed ('..'), bold ('==') or dotted ('~~')
        private const string LineCharacters = "[-.=~]";

        // e.g. '--', '.up.', '-[#red]-' or '-(0)-'
        private const string Line =
            "(?:"
            + LineCharacters
            + "+(?:"
            + MiddleToken
            + "+"
            + LineCharacters
            + "*)?|"
            + MiddleToken
            + "+"
            + LineCharacters
            + "+)";

        // an inline style after the link, e.g. '#line:red;line.bold;text:red'
        private const string InlineStyle = @"(?:\s+#\S+)?";

        private static readonly Regex Pattern = new Regex(
            @"^\s*"
                + LeftEndpoint
                + @"\s*"
                + Multiplicity
                + LeftDecoration
                + Line
                + RightDecoration
                + @"\s*"
                + Multiplicity
                + RightEndpoint
                + InlineStyle
                + @"\s*(?::.*)?$" // the optional description, e.g. ': uses'
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
            if (!match.Success || IsHidden(match))
            {
                yield break;
            }
            string left = match.Groups["left"].Value;
            string right = match.Groups["right"].Value;
            if (match.Groups["rightArrow"].Success || match.Groups["leftDiamond"].Success)
            {
                yield return new PlantUmlDependencyMatcher(left, right);
            }
            if (match.Groups["leftArrow"].Success || match.Groups["rightDiamond"].Success)
            {
                yield return new PlantUmlDependencyMatcher(right, left);
            }
        }

        // Hidden links only affect the layout, e.g. '-[hidden]->' or '-[#red,hidden]->',
        // see https://plantuml.com/class-diagram#help-on-layout
        private static bool IsHidden(Match match)
        {
            return match
                .Groups["style"]
                .Captures.Cast<Capture>()
                .SelectMany(style => style.Value.Split(','))
                .Any(attribute => attribute.Trim() == "hidden");
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
