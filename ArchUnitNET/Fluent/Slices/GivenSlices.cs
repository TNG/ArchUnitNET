using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    ///     Represents slices that have been matched by a namespace pattern.
    ///     Use <see cref="Should"/> to continue defining rule conditions, or
    ///     <see cref="GetObjects"/> to extract the slices for diagram generation.
    /// </summary>
    public class GivenSlices : IObjectProvider<Slice>
    {
        private readonly SliceRuleCreator _ruleCreator;

        /// <summary>
        ///     Initializes a new instance with the given rule creator.
        /// </summary>
        /// <param name="ruleCreator">The rule creator that accumulates the rule definition.</param>
        public GivenSlices(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        /// <inheritdoc />
        public string Description => _ruleCreator.Description;

        /// <summary>
        ///     Continues the rule definition with conditions that the matched slices must satisfy.
        /// </summary>
        /// <returns>
        ///     A <see cref="SlicesShould"/> that provides condition methods such as
        ///     <c>BeFreeOfCycles()</c> and <c>NotDependOnEachOther()</c>.
        /// </returns>
        public SlicesShould Should()
        {
            _ruleCreator.AddToDescription("should");
            return new SlicesShould(_ruleCreator);
        }

        /// <summary>
        ///     Evaluates the pattern against the given architecture and returns the resulting slices.
        ///     This is primarily used for PlantUML diagram generation.
        /// </summary>
        /// <param name="architecture">The architecture to evaluate against.</param>
        /// <returns>The slices produced by the pattern match, excluding ignored types.</returns>
        public IEnumerable<Slice> GetObjects(Architecture architecture)
        {
            return _ruleCreator.GetSlices(architecture);
        }

        /// <inheritdoc />
        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        )
        {
            return $"{multipleDescription} {Description}";
        }
    }
}
