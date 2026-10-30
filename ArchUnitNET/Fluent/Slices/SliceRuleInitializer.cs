// Adapted from https://github.com/TNG/ArchUnit/blob/main/archunit/src/main/java/com/tngtech/archunit/core/domain/PackageMatcher.java

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    /// Assigns types to slices by matching their namespaces against a pattern.
    /// </summary>
    /// <remarks>
    /// <para>A pattern is a namespace with these tokens in it:</para>
    /// <list type="table">
    ///     <item><term>(*)</term><description>captures exactly one segment</description></item>
    ///     <item><term>(**)</term><description>captures one or more segments</description></item>
    ///     <item><term>(A|B)</term><description>captures one of the alternatives</description></item>
    ///     <item><term>*</term><description>matches one segment without capturing it</description></item>
    ///     <item><term>..</term><description>matches zero or more segments</description></item>
    ///     <item><term>[A|B]</term><description>matches one of the alternatives</description></item>
    /// </list>
    /// <para>
    ///     A pattern must contain at least one capture group, may not mix (*) and (**), and may
    ///     use (**) at most once. Types whose namespace does not match are left out of the rule.
    /// </para>
    /// <para>
    ///     A slice is identified by what the capture groups captured, in group order
    ///     (<see cref="SliceIdentifier.Parts"/>): types that captured the same values share a
    ///     slice. "App.(*).(*)", "App.(*).Service.(*)" and "App.(*)..(*)" all put the types they
    ///     match in "Orders" and "Http" into the slice <c>[Orders, Http]</c>, while "App.(**)"
    ///     over "App.Orders.Http" captures the single value <c>[Orders.Http]</c>.
    /// </para>
    /// <para>
    ///     The slice is named after its parts, e.g. "Slice Orders - Http"; see
    ///     <see cref="SliceIdentifier.Description"/>.
    /// </para>
    /// </remarks>
    public class SliceRuleInitializer
    {
        private readonly SliceRuleCreator _ruleCreator;

        /// <summary>
        /// Initializes a new instance with the given rule creator.
        /// </summary>
        /// <param name="ruleCreator">The rule creator that accumulates the rule definition.</param>
        public SliceRuleInitializer(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        /// <summary>
        /// Groups types into slices by what the pattern captured.
        /// </summary>
        /// <param name="pattern">
        /// The namespace pattern; see the remarks on <see cref="SliceRuleInitializer"/>.
        /// </param>
        /// <exception cref="ArgumentException">The pattern is malformed.</exception>
        public GivenSlices Matching(string pattern)
        {
            return Matching(pattern, false);
        }

        /// <summary>
        /// Creates the same slices as <see cref="Matching"/>, which PlantUML diagrams nest in
        /// <c>package</c> blocks even without
        /// <see cref="Domain.PlantUml.Export.GenerationOptions.NestSlicesInPackages"/>.
        /// </summary>
        /// <param name="pattern">The namespace pattern, as for <see cref="Matching"/>.</param>
        /// <exception cref="ArgumentException">The pattern is malformed.</exception>
        [Obsolete(
            "Use Matching(pattern) and draw the slices with "
                + "new GenerationOptions { NestSlicesInPackages = true }. MatchingWithPackages "
                + "creates the same slices as Matching and will be removed in a future release."
        )]
        public GivenSlices MatchingWithPackages(string pattern)
        {
            return Matching(pattern, true);
        }

        private GivenSlices Matching(string pattern, bool nestByDefault)
        {
            var regex = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    type => AssignFunc(type, regex),
                    "matching \"" + pattern + "\"",
                    nestByDefault
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
        private static readonly Regex EscapedTwoDots = new Regex(
            @"\\\.\\\.",
            RegexOptions.Compiled
        );

        internal static Regex ConvertPatternToRegex(string pattern)
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
            return new Regex($"^{result}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Expands one ".." into a regex matching zero or more whole namespace segments,
        /// together with the dot that separates them from what surrounds the "..".
        /// </summary>
        /// <remarks>
        /// Consuming the boundary dot unconditionally is what keeps ".." on segment boundaries.
        /// An expression that is optional as a whole and brings its own dots also matches the
        /// empty string in the middle of a segment: "App.(*)..(*)" then splits "App.Slice3" into
        /// "Slice" and "3", and "App.(*)..Service" matches "App.MyService" as if "MyService"
        /// were two segments.
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

        /// <summary>
        /// Every "(...)" in the pattern is a capture group: "(*)", "(**)" and an alternation such
        /// as "(A|B)". Nesting is rejected up front, so no group contains another.
        /// </summary>
        private static readonly Regex CaptureGroup = new Regex(
            @"\([^()]*\)",
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
            if (!CaptureGroup.IsMatch(pattern))
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

        private static SliceIdentifier AssignFunc(IType type, Regex regex)
        {
            var namespc = type.Namespace.FullName;
            var match = regex.Match(namespc);
            if (!match.Success)
            {
                return SliceIdentifier.Ignore();
            }

            return SliceIdentifier.Of(Captures(match));
        }

        /// <summary>
        /// What the pattern's capture groups captured, in group order.
        /// </summary>
        internal static IReadOnlyList<string> Captures(Match match)
        {
            // Group 0 is the whole match; the rest are the pattern's capture groups in order.
            return match.Groups.Cast<Group>().Skip(1).Select(group => group.Value).ToList();
        }
    }
}
