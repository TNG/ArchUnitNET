using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    internal sealed class CombinedPredicate<TRuleType> : IPredicate<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly IPredicate<TRuleType> _leftPredicate;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IPredicate<TRuleType> _rightPredicate;

        public CombinedPredicate(
            IPredicate<TRuleType> leftPredicate,
            LogicalConjunction logicalConjunction,
            IPredicate<TRuleType> rightPredicate
        )
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
            _rightPredicate = rightPredicate;
        }

        public string Description =>
            $"{_leftPredicate.Description} {_logicalConjunction.Description} {_rightPredicate.Description}";

        public IEnumerable<TRuleType> GetMatchingObjects(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            if (!(objects is ICollection<TRuleType> objectCollection))
            {
                objectCollection = objects.ToList();
            }
            return _logicalConjunction.Evaluate(
                _leftPredicate.GetMatchingObjects(objectCollection, architecture),
                _rightPredicate.GetMatchingObjects(objectCollection, architecture)
            );
        }
    }
}
