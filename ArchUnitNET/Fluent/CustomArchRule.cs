using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class CustomArchRule : IArchRule
    {
        public string Description { get; }

        private readonly Func<
            Architecture,
            IArchRule,
            IEnumerable<EvaluationResult>
        > _evaluationFunc;

        public CustomArchRule(
            Func<Architecture, IArchRule, IEnumerable<EvaluationResult>> evaluationFunc,
            string description
        )
        {
            _evaluationFunc = evaluationFunc;
            Description = description;
        }

        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _evaluationFunc.Invoke(architecture, this);
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
    }
}
