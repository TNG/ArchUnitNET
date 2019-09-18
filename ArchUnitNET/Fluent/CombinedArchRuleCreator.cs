using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRuleCreator<T> : ArchRuleCreator<T> where T : ICanBeAnalyzed
    {
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IArchRuleCreator _oldArchRuleCreator;

        public CombinedArchRuleCreator(IArchRuleCreator archRuleCreator, LogicalConjunction logicalConjunction,
            ObjectProvider<T> objectProvider) : base(objectProvider)
        {
            _oldArchRuleCreator = archRuleCreator;
            _logicalConjunction = logicalConjunction;
        }

        public override string Description => _oldArchRuleCreator.Description + " " + _logicalConjunction.Description +
                                              " " + base.Description;

        public override bool Check(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_oldArchRuleCreator.Check(architecture),
                base.Check(architecture));
        }

        public override IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _oldArchRuleCreator.Evaluate(architecture).Concat(base.Evaluate(architecture));
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(CombinedArchRuleCreator<T> other)
        {
            return string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((CombinedArchRuleCreator<T>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}