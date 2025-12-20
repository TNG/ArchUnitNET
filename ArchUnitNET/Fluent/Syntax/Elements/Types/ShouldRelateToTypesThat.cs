using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class ShouldRelateToTypesThat<TNextElement, TRuleType>
        : AddTypePredicate<TNextElement, IType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, IType> _relationCondition;

        public ShouldRelateToTypesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, IType> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<IType> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<IType>(ObjectProvider, predicate)
                )
            );
    }
}
