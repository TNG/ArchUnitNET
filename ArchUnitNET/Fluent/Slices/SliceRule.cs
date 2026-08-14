using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    ///     A completed slice architecture rule that can be evaluated against an architecture.
    ///     Implements <see cref="IArchRule"/> and can be combined with other rules using
    ///     <see cref="And()"/> and <see cref="Or()"/>.
    /// </summary>
    public class SliceRule : IArchRule
    {
        private readonly SliceRuleCreator _ruleCreator;

        /// <summary>
        ///     Initializes a new instance with the given rule creator.
        /// </summary>
        /// <param name="ruleCreator">The rule creator that contains the complete rule definition.</param>
        public SliceRule(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        /// <inheritdoc />
        public string Description => _ruleCreator.Description;

        /// <inheritdoc />
        public bool HasNoViolations(Architecture architecture)
        {
            return _ruleCreator.HasNoViolations(architecture);
        }

        /// <inheritdoc />
        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _ruleCreator.Evaluate(architecture);
        }

        /// <inheritdoc />
        public CombinedArchRuleDefinition And()
        {
            return new CombinedArchRuleDefinition(_ruleCreator, LogicalConjunctionDefinition.And);
        }

        /// <inheritdoc />
        public CombinedArchRuleDefinition Or()
        {
            return new CombinedArchRuleDefinition(_ruleCreator, LogicalConjunctionDefinition.Or);
        }

        /// <inheritdoc />
        public IArchRule And(IArchRule archRule)
        {
            return new CombinedArchRule(_ruleCreator, LogicalConjunctionDefinition.And, archRule);
        }

        /// <inheritdoc />
        public IArchRule Or(IArchRule archRule)
        {
            return new CombinedArchRule(_ruleCreator, LogicalConjunctionDefinition.Or, archRule);
        }
    }
}
