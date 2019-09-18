using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRuleCreator<T> : ArchRuleCreator<T> where T : ICanBeAnalyzed
    {
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IArchRuleCreator _oldArchRuleCreator;

        public CombinedArchRuleCreator(IArchRuleCreator archRuleCreator, LogicalConjunction logicalConjunction,
            Func<Architecture, IEnumerable<T>> objectsToBeAnalyzed, string description) : base(objectsToBeAnalyzed,
            archRuleCreator.Description + " " + logicalConjunction.Description + " " + description)
        {
            _oldArchRuleCreator = archRuleCreator;
            _logicalConjunction = logicalConjunction;
        }

        public override bool Check(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_oldArchRuleCreator.Check(architecture),
                base.Check(architecture));
        }
    }
}