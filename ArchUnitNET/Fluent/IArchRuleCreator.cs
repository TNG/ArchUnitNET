using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent
{
    public interface IArchRuleCreator<TRuleType> : ICanBeEvaluated
        where TRuleType : ICanBeAnalyzed
    {
        void AddPredicate(IPredicate<TRuleType> predicate);
        void AddPredicateConjunction(LogicalConjunction logicalConjunction);
        void AddCondition(IOrderedCondition<TRuleType> condition);
        void AddConditionConjunction(LogicalConjunction logicalConjunction);
        void AddConditionReason(string reason);
        void AddPredicateReason(string reason);

        void BeginComplexCondition<TRelatedType>(
            IObjectProvider<TRelatedType> relatedObjects,
            RelationCondition<TRuleType, TRelatedType> relationCondition
        )
            where TRelatedType : ICanBeAnalyzed;

        void ContinueComplexCondition<TRelatedType>(IPredicate<TRelatedType> predicate)
            where TRelatedType : ICanBeAnalyzed;

        IEnumerable<TRuleType> GetAnalyzedObjects(Architecture architecture);

        void SetCustomPredicateDescription(string description);
        void SetCustomConditionDescription(string description);

        bool RequirePositiveResults { get; set; }
    }
}
