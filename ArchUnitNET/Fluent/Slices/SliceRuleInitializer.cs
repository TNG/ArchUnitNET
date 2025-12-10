// Adapted from https://github.com/TNG/ArchUnit/blob/main/archunit/src/main/java/com/tngtech/archunit/core/domain/PackageMatcher.java

using System;
using System.Linq;
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
            var (regex, asterisk) = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    t => AssignFunc(t, regex, asterisk),
                    "matching \"" + pattern + "\""
                )
            );
            return new GivenSlices(_ruleCreator);
        }

        public GivenSlices MatchingWithPackages(string pattern)
        {
            var (regex, asterisk) = ConvertPatternToRegex(pattern);
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    type => AssignFunc(type, regex, asterisk, true),
                    "matching \"" + pattern + "\""
                )
            );
            return new GivenSlices(_ruleCreator);
        }

        private static (Regex regex, int? asterisk) ConvertPatternToRegex(string pattern)
        {
            AssertPatternIsValid(pattern);
            const string twoStarCaptureLiteral = "(**)";
            const string twoStarRegexMarker = "#%#%#";
            const string twoStarCaptureRegex = @"(\w+(?:\.\w+)*)"; // Captures one or more segments with dots
            const string singleStarCaptureRegex = @"(\w+)"; // Captures single segment without dots
            const string twoDotsRegex = @"(?:(?:^\w*)?\.(?:\w+\.)*(?:\w*$)?)?"; // .. pattern for zero or more packages
            var result = Regex
                .Replace(pattern, @"\[(.*?)]", "(?:$1)")
                .Replace(twoStarCaptureLiteral, twoStarRegexMarker)
                .Replace("(*)", singleStarCaptureRegex)
                .Replace("*", @"\w+")
                .Replace(".", @"\.")
                .Replace(@"\.\.", twoDotsRegex)
                .Replace(twoStarRegexMarker, twoStarCaptureRegex);
            var countOfSingleAsterisk = pattern.Contains("(**)")
                ? (int?)null
                : pattern.Split(new[] { "(*)" }, StringSplitOptions.None).Length - 1;
            return (new Regex($"^{result}$", RegexOptions.Compiled), countOfSingleAsterisk);
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
            int? countOfSingleAsterisk,
            bool fullName = false
        )
        {
            var namespc = type.Namespace.FullName;
            var match = regex.Match(namespc);
            if (!match.Success)
            {
                return SliceIdentifier.Ignore();
            }

            // Extract captured group (first capture group in the pattern)
            if (match.Groups.Count < 2)
            {
                throw new ArgumentException(
                    $"\"{type.FullName}\" is not clearly assignable to a slice"
                );
            }

            var capturedValue = match.Groups[1].Value;

            // For pattern matching: Get the prefix before the capture group for fullName mode
            if (fullName)
            {
                // Find where the captured value starts in the namespace
                var prefixEndIndex = namespc.IndexOf(capturedValue, StringComparison.Ordinal);
                var slicePrefix = prefixEndIndex > 0 ? namespc.Substring(0, prefixEndIndex) : "";
                return SliceIdentifier.Of(
                    slicePrefix + capturedValue,
                    countOfSingleAsterisk,
                    slicePrefix
                );
            }

            return SliceIdentifier.Of(capturedValue, countOfSingleAsterisk);
        }
    }
}
