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

        private static readonly Regex BlockOpenPattern = new Regex(@"\{\s*$");

        private static readonly Regex BlockClosePattern = new Regex(@"^\s*}\s*$");

        internal static bool IsBlockOpen(string line)
        {
            return BlockOpenPattern.IsMatch(line);
        }

        internal static bool IsBlockClose(string line)
        {
            return BlockClosePattern.IsMatch(line);
        }
    }

    // Matches a link between two components, see e.g.
    // https://plantuml.com/class-diagram#relations-between-classes and
    // https://plantuml.com/deployment-diagram#linking-or-arrow
    internal class PlantUmlDependencyMatcher
    {
        // '[Some Name]', or a name or alias without brackets, e.g. 'some-alias' or 'Some.Name'
        private const string PlainName = @"[^\s\[\]<>"":*#+|^@(){}\\~=]+?";
        private const string LeftEndpoint =
            @"(?:(?<leftBracket>\[)(?<left>[^\[\]]+)\]|(?<left>" + PlainName + "))";
        private const string RightEndpoint =
            @"(?:(?<rightBracket>\[)(?<right>[^\[\]]+)\]|(?<right>" + PlainName + "))";

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

        private PlantUmlDependencyMatcher(PlantUmlEndpoint origin, PlantUmlEndpoint target)
        {
            Origin = origin;
            Target = target;
        }

        internal PlantUmlEndpoint Origin { get; }
        internal PlantUmlEndpoint Target { get; }

        internal static IEnumerable<PlantUmlDependencyMatcher> TryParse(string line)
        {
            Match match = Pattern.Match(line);
            if (!match.Success || IsHidden(match))
            {
                yield break;
            }
            var left = new PlantUmlEndpoint(match, "left");
            var right = new PlantUmlEndpoint(match, "right");
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

    // One side of a link, e.g. '[Some Name]' or 'some-alias'
    internal class PlantUmlEndpoint
    {
        internal PlantUmlEndpoint(Match match, string side)
        {
            Name = match.Groups[side].Value;
            MayReferToGroup = !match.Groups[side + "Bracket"].Success;
        }

        internal string Name { get; }

        // A name in brackets always refers to a component, a name without brackets may also
        // refer to a group.
        internal bool MayReferToGroup { get; }
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

    internal class PlantUmlGroupMatcher
    {
        private static readonly Regex Pattern = new Regex(
            @"^\s*(?:package|node|folder|frame|cloud|database)\s+"
                + @"(?:""(?<name>[^""]+)""|(?<name>[^\s{""<]+))\s*"
                + @"(?:as\s+""?(?<alias>[^""\s{<]+)""?\s*)?"
                // stereotypes on a group are accepted, but do not assign any types to it
                + PlantUmlPatterns.Stereotypes
                + PlantUmlPatterns.Color
                + @"\s*\{\s*$"
        );

        private PlantUmlGroupMatcher(Match match)
        {
            Name = match.Groups["name"].Value;
            Alias = match.Groups["alias"].Success ? match.Groups["alias"].Value : null;
        }

        internal string Name { get; }
        internal string Alias { get; } // null without 'as'

        internal static PlantUmlGroupMatcher TryParse(string line)
        {
            Match match = Pattern.Match(line);
            return match.Success ? new PlantUmlGroupMatcher(match) : null;
        }
    }
}
