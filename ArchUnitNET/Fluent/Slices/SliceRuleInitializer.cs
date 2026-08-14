// Adapted from https://github.com/TNG/ArchUnit/blob/main/archunit/src/main/java/com/tngtech/archunit/core/domain/PackageMatcher.java

using System;
using System.Text;
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
    ///     The slice's name is what the capture groups matched, joined with ".", so
    ///     "App.(*).(*)" names a slice "Orders.Http". See <see cref="SliceIdentifier.Description"/>.
    /// </para>
    /// <para>
    ///     The name is also the slice's identity: types with the same name share a slice. That
    ///     groups them exactly by what they captured; see <see cref="SliceName"/>.
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
        /// Groups types into slices named after what the pattern captured.
        /// </summary>
        /// <param name="pattern">
        /// The namespace pattern; see the remarks on <see cref="SliceRuleInitializer"/> and
        /// https://www.archunit.org/userguide/html/000_Index.html#_slices for more examples.
        /// </param>
        /// <exception cref="ArgumentException">The pattern is malformed.</exception>
        public GivenSlices Matching(string pattern)
        {
            var regex = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(type => AssignFunc(type, regex), "matching \"" + pattern + "\"")
            );
            return new GivenSlices(_ruleCreator);
        }

        /// <summary>
        /// Like <see cref="Matching"/>, but keeps the namespace the pattern matched before the
        /// first capture group. Slice names are then fully qualified, and PlantUML diagrams nest
        /// the slices in <c>package</c> blocks along that namespace.
        /// </summary>
        /// <param name="pattern">
        /// The namespace pattern; see the remarks on <see cref="SliceRuleInitializer"/>.
        /// </param>
        /// <exception cref="ArgumentException">The pattern is malformed.</exception>
        public GivenSlices MatchingWithPackages(string pattern)
        {
            var regex = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    type => AssignFunc(type, regex, true),
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
        /// This deviates from PackageMatcher.TWO_DOTS_REGEX in ArchUnit, which is
        /// "(?:(?:^\w*)?\.(?:\w+\.)*(?:\w*$)?)?". That expression is optional as a whole and
        /// brings its own dots, so it also matches the empty string in the middle of a segment:
        /// "App.(*)..(*)" then splits "App.Slice3" into "Slice" and "3", and "App.(*)..Service"
        /// matches "App.MyService" as if "MyService" were two segments. Consuming the boundary
        /// dot unconditionally is what keeps ".." on segment boundaries.
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
        /// Every "(...)" in the pattern is a capture group: "(*)", "(**)" and, as in ArchUnit, an
        /// alternation such as "(A|B)". Nesting is rejected up front, so no group contains another.
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

        private static SliceIdentifier AssignFunc(IType type, Regex regex, bool fullName = false)
        {
            var namespc = type.Namespace.FullName;
            var match = regex.Match(namespc);
            if (!match.Success)
            {
                return SliceIdentifier.Ignore();
            }

            var name = SliceName(match);
            if (!fullName)
            {
                return SliceIdentifier.Of(name);
            }

            // An empty prefix means the pattern starts with its first capture group; there is no
            // namespace to nest the slice under, so report none rather than an empty one.
            var slicePrefix = namespc.Substring(0, match.Groups[1].Index);
            return SliceIdentifier.Of(slicePrefix + name, slicePrefix == "" ? null : slicePrefix);
        }

        /// <summary>
        /// The slice's name: what the capture groups matched, joined with ".".
        /// </summary>
        /// <remarks>
        /// The name is also the slice's identity, so two types share a slice exactly when they get
        /// the same name. That groups them by what they captured, because "(*)" captures no dots
        /// and "(**)" appears at most once: the captures can be read back from the name in only
        /// one way. Joining with "." matches the exporter, which splits slice names on "." to
        /// nest package blocks, and makes "App.(*).(*)" name a grouping the same way as
        /// "App.(**)".
        /// </remarks>
        internal static string SliceName(Match match)
        {
            // Group 0 is the whole match; the rest are the pattern's capture groups in order.
            var name = new StringBuilder(match.Groups[1].Value);
            for (var i = 2; i < match.Groups.Count; i++)
            {
                name.Append('.').Append(match.Groups[i].Value);
            }

            return name.ToString();
        }
    }
}
