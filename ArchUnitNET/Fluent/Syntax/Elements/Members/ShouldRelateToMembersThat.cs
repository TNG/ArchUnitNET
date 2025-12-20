using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
    public class ShouldRelateToMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : AddMemberPredicate<TRuleTypeShouldConjunction, IMember>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TRuleTypeShouldConjunction,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, IMember> _relationCondition;

        public ShouldRelateToMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> relatedObjectProvider,
            PartialConditionConjunction<
                TRuleTypeShouldConjunction,
                TRuleType
            > partialConditionConjunction,
            RelationCondition<TRuleType, IMember> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TRuleTypeShouldConjunction CreateNextElement(
            IPredicate<IMember> predicate
        ) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<IMember>(ObjectProvider, predicate)
                )
            );
    }
}
