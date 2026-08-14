using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    ///     Internal orchestrator that wires together the slice assignment (how types map to slices)
    ///     and the evaluation function (what condition to check on those slices). Built up
    ///     incrementally by the fluent API chain.
    /// </summary>
    public class SliceRuleCreator : ICanBeEvaluated
    {
        private Func<
            IEnumerable<Slice>,
            ICanBeEvaluated,
            Architecture,
            IEnumerable<EvaluationResult>
        > _evaluationFunc;
        private SliceAssignment _sliceAssignment;

        /// <summary>
        ///     Initializes a new instance with the default description "Slices".
        /// </summary>
        public SliceRuleCreator()
        {
            Description = "Slices";
        }

        /// <summary>
        ///     The human-readable description of the rule being built, accumulated from each
        ///     fluent method call (e.g. <c>"Slices matching \"App.(*)\" should be free of cycles"</c>).
        /// </summary>
        public string Description { get; private set; }

        /// <inheritdoc />
        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        /// <inheritdoc />
        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _evaluationFunc(GetSlices(architecture), this, architecture);
        }

        /// <summary>
        ///     Sets the slice assignment that defines how types are grouped into slices.
        /// </summary>
        /// <param name="sliceAssignment">
        ///     The assignment containing the pattern-matching function and its description.
        /// </param>
        public void SetSliceAssignment(SliceAssignment sliceAssignment)
        {
            _sliceAssignment = sliceAssignment;
            AddToDescription(sliceAssignment.Description);
        }

        /// <summary>
        ///     Sets the evaluation function that checks the condition on the slices.
        /// </summary>
        /// <param name="evaluationFunc">
        ///     A function that takes the slices, the rule being evaluated, and the architecture,
        ///     and returns evaluation results for each check.
        /// </param>
        public void SetEvaluationFunction(
            Func<
                IEnumerable<Slice>,
                ICanBeEvaluated,
                Architecture,
                IEnumerable<EvaluationResult>
            > evaluationFunc
        )
        {
            _evaluationFunc = evaluationFunc;
        }

        /// <summary>
        ///     Appends a word or phrase to the rule description.
        /// </summary>
        /// <param name="description">The text to append (leading/trailing whitespace is trimmed).</param>
        public void AddToDescription(string description)
        {
            Description += " " + description.Trim();
        }

        /// <summary>
        ///     Evaluates the slice assignment against the architecture and returns all non-ignored
        ///     slices. Types that did not match the pattern are excluded.
        /// </summary>
        /// <param name="architecture">The architecture to evaluate against.</param>
        /// <returns>The matched slices, excluding ignored types.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <see cref="SetSliceAssignment"/> has not been called before this method.
        /// </exception>
        public IEnumerable<Slice> GetSlices(Architecture architecture)
        {
            if (_sliceAssignment == null)
            {
                throw new InvalidOperationException(
                    "The Slice Assignment has to be set before GetSlices() can be called."
                );
            }

            return _sliceAssignment
                .Apply(architecture.Types)
                .Where(slice => !slice.Identifier.Ignored);
        }
    }
}
