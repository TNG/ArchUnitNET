using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceRule : IArchRule
    {
        private readonly SliceAssignment _sliceAssignment;
        private readonly Func<IEnumerable<Slice>, ICanBeEvaluated, Architecture, IEnumerable<EvaluationResult>> _evaluationFunc;
        private readonly string _description;

        public SliceRule(
            SliceAssignment sliceAssignment,
            Func<IEnumerable<Slice>, ICanBeEvaluated, Architecture, IEnumerable<EvaluationResult>> evaluationFunc,
            string description
        )
        {
            _sliceAssignment = sliceAssignment;
            _evaluationFunc = evaluationFunc;
            _description = description;
        }

        public string Description => "Slices " + _sliceAssignment.Description + " should " + _description;

        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            var slices = GetSlices(architecture);
            return _evaluationFunc(slices, this, architecture);
        }

        public CombinedArchRuleDefinition And()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.And);
        }

        public CombinedArchRuleDefinition Or()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.Or);
        }

        public IArchRule And(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.And, archRule);
        }

        public IArchRule Or(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.Or, archRule);
        }

        private IEnumerable<Slice> GetSlices(Architecture architecture)
        {
            return _sliceAssignment
                .Apply(architecture.Types)
                .Where(slice => !slice.Identifier.Ignored);
        }
    }
}
