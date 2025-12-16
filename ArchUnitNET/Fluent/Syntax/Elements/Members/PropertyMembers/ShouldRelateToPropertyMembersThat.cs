using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public sealed class ShouldRelateToPropertyMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : AddMemberPredicate<PropertyMember, TRuleType, TRuleTypeShouldConjunction>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToPropertyMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TRuleTypeShouldConjunction CreateNextElement(
            IPredicate<PropertyMember> predicate
        )
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
