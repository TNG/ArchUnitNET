using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public sealed class ShouldRelateToMethodMembersThat<TNextElement, TRuleType>
        : AddMethodMemberPredicate<TNextElement>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, MethodMember> _relationCondition;

        public ShouldRelateToMethodMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, MethodMember> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<MethodMember> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<MethodMember>(ObjectProvider, predicate)
                )
            );
    }
}
