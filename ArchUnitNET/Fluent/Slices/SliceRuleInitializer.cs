// Adapted from https://github.com/TNG/ArchUnit/blob/main/archunit/src/main/java/com/tngtech/archunit/core/domain/PackageMatcher.java

using System;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceRuleInitializer
    {
        private readonly SliceRuleCreator _ruleCreator;

        public SliceRuleInitializer(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        /// <summary>
        /// </summary>
        /// <param name="pattern">
        ///     check https://www.archunit.org/userguide/html/000_Index.html#_slices for examples for pattern
        ///     usage
        /// </param>
        /// <returns></returns>
        public GivenSlices Matching(string pattern)
        {
            var (regex, separators) = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    type => AssignFunc(type, regex, separators),
                    "matching \"" + pattern + "\""
                )
            );
            return new GivenSlices(_ruleCreator);
        }

        public GivenSlices MatchingWithPackages(string pattern)
        {
            var (regex, separators) = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    type => AssignFunc(type, regex, separators, true),
                    "matching \"" + pattern + "\""
                )
            );
            return new GivenSlices(_ruleCreator);
        }

        private const string TwoStarCaptureLiteral = "(**)";
        private const string TwoStarRegexMarker = "#%#%#";

        /// <summary>Captures one or more namespace segments, separated by dots.</summary>
        private const string TwoStarCaptureRegex = @"(\w+(?:\.\w+)*)";

        /// <summary>Captures exactly one namespace segment.</summary>
        private const string SingleStarCaptureRegex = @"(\w+)";

        /// <summary>Matches an escaped ".." in an already escaped pattern.</summary>
        private static readonly Regex EscapedTwoDots = new Regex(@"\\\.\\\.", RegexOptions.Compiled);

        private static (Regex regex, string[] separators) ConvertPatternToRegex(string pattern)
        {
            AssertPatternIsValid(pattern);
            var escaped = Regex
                .Replace(pattern, @"\[(.*?)]", "(?:$1)")
                .Replace(TwoStarCaptureLiteral, TwoStarRegexMarker)
                .Replace("(*)", SingleStarCaptureRegex)
                .Replace("*", @"\w+")
                .Replace(".", @"\.");
            var result = EscapedTwoDots
                .Replace(escaped, match => TwoDotsRegex(match, escaped))
                .Replace(TwoStarRegexMarker, TwoStarCaptureRegex);
            return (new Regex($"^{result}$", RegexOptions.Compiled), NameSeparators(pattern));
        }

        private static readonly Regex CaptureGroup = new Regex(@"\(\*\*?\)", RegexOptions.Compiled);

        private static readonly Regex Alternation = new Regex(@"\[[^\]]*]", RegexOptions.Compiled);

        /// <summary>
        ///     The pattern text between each pair of adjacent capture groups, used to join the
        ///     captured parts into the slice name. Everything before the first group and after the
        ///     last one selects which types belong to a slice rather than naming it, so it is left
        ///     out: that is what keeps "App.(*)" naming a slice "Orders" rather than "App.Orders".
        /// </summary>
        /// <remarks>
        ///     Keeping the pattern's own text means the name says what sits between the captures:
        ///     "App.(*).Service.(*)" names a slice "Orders.Service.Http", which is the real
        ///     namespace, while "App.(*)..(*)" names one "Orders..Http", which visibly is not.
        ///     Alternations become "*" because a PlantUML name may not contain "[" or "]", and
        ///     because -- like "*" -- which alternative matched is not part of the slice's identity.
        ///     The separators come from the pattern, not from the match, so every type the pattern
        ///     groups together keeps the same name.
        /// </remarks>
        private static string[] NameSeparators(string pattern)
        {
            var groups = CaptureGroup.Matches(pattern);
            var separators = new string[groups.Count - 1];
            for (var i = 0; i < separators.Length; i++)
            {
                var start = groups[i].Index + groups[i].Length;
                var between = pattern.Substring(start, groups[i + 1].Index - start);
                separators[i] = Alternation.Replace(between, "*");
            }

            return separators;
        }

        /// <summary>
        ///     Expands one ".." into a regex matching zero or more whole namespace segments,
        ///     together with the dot that separates them from what surrounds the "..".
        /// </summary>
        /// <remarks>
        ///     This deviates from PackageMatcher.TWO_DOTS_REGEX in ArchUnit, which is
        ///     "(?:(?:^\w*)?\.(?:\w+\.)*(?:\w*$)?)?". That expression is optional as a whole and
        ///     brings its own dots, so it also matches the empty string in the middle of a segment:
        ///     "App.(*)..(*)" then splits "App.Slice3" into "Slice" and "3", and "App.(*)..Service"
        ///     matches "App.MyService" as if "MyService" were two segments. Consuming the boundary
        ///     dot unconditionally is what keeps ".." on segment boundaries.
        /// </remarks>
        private static string TwoDotsRegex(Match match, string escapedPattern)
        {
            if (match.Index == 0)
            {
                return @"(?:\w+\.)*";
            }

            return match.Index + match.Length == escapedPattern.Length
                ? @"(?:\.\w+)*"
                : @"\.(?:\w+\.)*";
        }

        private static readonly Regex IllegalAlternation = new Regex(
            @"\[[^|]*\]",
            RegexOptions.Compiled
        );

        private static readonly Regex IllegalNestedGroup = new Regex(
            @"\([^)]*\(|\([^)]*\[|\[[^\]]*\(|\[[^\]]*\[",
            RegexOptions.Compiled
        );

        private static void AssertPatternIsValid(string pattern)
        {
            if (pattern.Contains("..."))
            {
                throw new ArgumentException(
                    "Pattern may not contain more than two '.' in a row",
                    nameof(pattern)
                );
            }

            if (pattern.Replace("(**)", "").Contains("**"))
            {
                throw new ArgumentException(
                    "Pattern may not contain more than one '*' in a row",
                    nameof(pattern)
                );
            }

            if (pattern.Contains("(..)"))
            {
                throw new ArgumentException(
                    "Pattern does not support capturing via (..), use (**) instead",
                    nameof(pattern)
                );
            }

            if (IllegalAlternation.IsMatch(pattern))
            {
                throw new ArgumentException(
                    "Pattern does not allow alternation brackets '[]' without specifying any alternative via '|' inside",
                    nameof(pattern)
                );
            }

            if (ContainsToplevelAlternation(pattern))
            {
                throw new ArgumentException(
                    "Pattern only supports '|' inside of '[]' or '()'",
                    nameof(pattern)
                );
            }

            if (IllegalNestedGroup.IsMatch(pattern))
            {
                throw new ArgumentException(
                    "Namespace identifier does not support nesting '()' or '[]' within other '()' or '[]'",
                    nameof(pattern)
                );
            }

            if (pattern.Contains("(*)") && pattern.Contains("(**)"))
            {
                throw new ArgumentException("Patterns for Slices can't contain both (*) and (**).");
            }

            if (
                pattern.IndexOf("(**)", StringComparison.Ordinal)
                != pattern.LastIndexOf("(**)", StringComparison.Ordinal)
            )
            {
                throw new ArgumentException("Patterns for Slices can contain (**) only once.");
            }

            // Checked last on purpose: a pattern that is malformed *and* has no capture group
            // should report the specific problem rather than this catch-all.
            if (!pattern.Contains("(*)") && !pattern.Contains("(**)"))
            {
                throw new ArgumentException("Patterns for Slices have to contain (*) or (**).");
            }
        }

        private static bool ContainsToplevelAlternation(string pattern)
        {
            var depth = 0;
            foreach (var c in pattern)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                        depth++;
                        break;
                    case ')':
                    case ']':
                        depth--;
                        break;
                    case '|' when depth == 0:
                        return true;
                }
            }

            return false;
        }

        private static SliceIdentifier AssignFunc(
            IType type,
            Regex regex,
            string[] separators,
            bool fullName = false
        )
        {
            var namespc = type.Namespace.FullName;
            var match = regex.Match(namespc);
            if (!match.Success)
            {
                return SliceIdentifier.Ignore();
            }

            // Group 0 is the whole match; the rest are the pattern's capture groups in order.
            var parts = new string[match.Groups.Count - 1];
            for (var i = 0; i < parts.Length; i++)
            {
                parts[i] = match.Groups[i + 1].Value;
            }

            if (!fullName)
            {
                return SliceIdentifier.Of(parts, separators);
            }

            // An empty prefix means the pattern starts with its first capture group; there is no
            // namespace to nest the slice under, so report none rather than an empty one.
            var slicePrefix = namespc.Substring(0, match.Groups[1].Index);
            parts[0] = slicePrefix + parts[0];
            return SliceIdentifier.Of(parts, separators, slicePrefix == "" ? null : slicePrefix);
        }
    }
}
