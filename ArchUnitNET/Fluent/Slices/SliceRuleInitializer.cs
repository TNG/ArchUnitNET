using System;
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
            return Matching(pattern, false);
        }

        /// <summary>
        /// Creates the same slices as <see cref="Matching"/>, which PlantUML diagrams nest in
        /// <c>package</c> blocks even without
        /// <see cref="Domain.PlantUml.Export.GenerationOptions.NestSlicesInPackages"/>.
        /// </summary>
        /// <param name="pattern">The namespace pattern, as for <see cref="Matching"/>.</param>
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
            _ruleCreator.SetSliceAssignment(
                new SliceAssignment(
                    t =>
                    {
                        var parseResult = Parse(pattern);
                        return AssignFunc(t, parseResult.pattern, parseResult.asterisk);
                    },
                    "matching \"" + pattern + "\"",
                    nestByDefault
                )
            );
            return new GivenSlices(_ruleCreator);
        }

        private static (string pattern, int? asterisk) Parse(string pattern)
        {
            var indexOfAsteriskInPattern = pattern.IndexOf("(*", StringComparison.Ordinal);
            var containsSingleAsterisk = pattern.Contains("(*)");
            var containsDoubleAsterisk = pattern.Contains("(**)");

            if (!containsSingleAsterisk && !containsDoubleAsterisk)
            {
                throw new ArgumentException("Patterns for Slices have to contain (*) or (**).");
            }

            if (containsDoubleAsterisk && containsSingleAsterisk)
            {
                throw new ArgumentException("Patterns for Slices can't contain both (*) and (**).");
            }

            if (
                pattern.IndexOf("(**", StringComparison.Ordinal)
                != pattern.LastIndexOf("(**", StringComparison.Ordinal)
            )
            {
                throw new ArgumentException("Patterns for Slices can contain (**) only once.");
            }

            if (containsDoubleAsterisk)
            {
                return (pattern, null);
            }

            var countOfSingleAsterisk =
                pattern.Split(new[] { "(*)" }, StringSplitOptions.None).Length - 1;
            pattern = pattern.Remove(indexOfAsteriskInPattern) + "(**).";
            return (pattern, countOfSingleAsterisk);
        }

        private static SliceIdentifier AssignFunc(
            IType type,
            string pattern,
            int? countOfSingleAsterisk
        )
        {
            var indexOfAsteriskInPattern = pattern.IndexOf("(*", StringComparison.Ordinal);

            var namespc = type.Namespace.FullName;
            var slicePrefix = pattern.Remove(indexOfAsteriskInPattern);
            var slicePostfix = pattern.Substring(
                pattern.IndexOf("*)", StringComparison.Ordinal) + 2
            );

            if (slicePrefix.StartsWith("."))
            {
                slicePrefix = slicePrefix.Substring(1);
                if (!namespc.Contains(slicePrefix))
                {
                    return SliceIdentifier.Ignore();
                }
            }
            else if (!namespc.StartsWith(slicePrefix))
            {
                return SliceIdentifier.Ignore();
            }

            if (slicePostfix.EndsWith("."))
            {
                slicePostfix = slicePostfix.Remove(slicePostfix.Length - 1);
                if (
                    !namespc
                        .Substring(
                            namespc.IndexOf(slicePrefix, StringComparison.Ordinal)
                                + slicePrefix.Length
                        )
                        .Contains(slicePostfix)
                )
                {
                    return SliceIdentifier.Ignore();
                }
            }
            else if (!namespc.EndsWith(slicePostfix))
            {
                return SliceIdentifier.Ignore();
            }

            var sliceString = namespc;
            var matchedPrefix = "";

            if (slicePrefix != "")
            {
                var prefixEnd =
                    namespc.IndexOf(slicePrefix, StringComparison.Ordinal) + slicePrefix.Length;
                matchedPrefix = namespc.Substring(0, prefixEnd);
                sliceString = namespc.Substring(prefixEnd).TrimStart('.');
            }

            if (!sliceString.Contains(slicePostfix))
            {
                throw new ArgumentException(
                    "\""
                        + type.FullName
                        + "\" is not clearly assignable to a slice with the pattern: \""
                        + pattern
                        + "\""
                );
            }

            if (slicePostfix != "")
            {
                sliceString = sliceString.Remove(
                    sliceString.IndexOf(slicePostfix, StringComparison.Ordinal)
                );
            }

            // What the pattern captured is both what identifies the slice and the path a
            // diagram nests it along.
            return SliceIdentifier.OfMatch(
                sliceString,
                countOfSingleAsterisk,
                matchedPrefix,
                sliceString
            );
        }
    }
}
