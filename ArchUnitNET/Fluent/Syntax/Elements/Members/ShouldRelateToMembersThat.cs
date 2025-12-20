using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
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
