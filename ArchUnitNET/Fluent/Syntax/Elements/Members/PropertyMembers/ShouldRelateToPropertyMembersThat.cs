using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public sealed class ShouldRelateToPropertyMembersThat<TNextElement, TRuleType>
        : AddPropertyMemberPredicate<TNextElement>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, PropertyMember> _relationCondition;

        public ShouldRelateToPropertyMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, PropertyMember> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<PropertyMember> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<PropertyMember>(ObjectProvider, predicate)
                )
            );
    }
}
