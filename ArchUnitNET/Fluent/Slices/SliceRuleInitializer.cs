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
            var regex = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(type => AssignFunc(type, regex), "matching \"" + pattern + "\"")
            );
            return new GivenSlices(_ruleCreator);
        }

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

        /// <summary>Matches zero or more namespace segments.</summary>
        private const string TwoDotsRegex = @"(?:(?:^\w*)?\.(?:\w+\.)*(?:\w*$)?)?";

        private static Regex ConvertPatternToRegex(string pattern)
        {
            AssertPatternIsValid(pattern);
            var result = Regex
                .Replace(pattern, @"\[(.*?)]", "(?:$1)")
                .Replace(TwoStarCaptureLiteral, TwoStarRegexMarker)
                .Replace("(*)", SingleStarCaptureRegex)
                .Replace("*", @"\w+")
                .Replace(".", @"\.")
                .Replace(@"\.\.", TwoDotsRegex)
                .Replace(TwoStarRegexMarker, TwoStarCaptureRegex);
            return new Regex($"^{result}$", RegexOptions.Compiled);
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

        private static SliceIdentifier AssignFunc(IType type, Regex regex, bool fullName = false)
        {
            var namespc = type.Namespace.FullName;
            var match = regex.Match(namespc);
            if (!match.Success)
            {
                return SliceIdentifier.Ignore();
            }

            var capturedValue = match.Groups[1].Value;
            if (!fullName)
            {
                return SliceIdentifier.Of(capturedValue);
            }

            var slicePrefix = namespc.Substring(0, match.Groups[1].Index);
            return SliceIdentifier.Of(slicePrefix + capturedValue, slicePrefix);
        }
    }
}
