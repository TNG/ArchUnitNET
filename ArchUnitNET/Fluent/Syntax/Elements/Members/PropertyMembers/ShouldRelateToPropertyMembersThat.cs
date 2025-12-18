using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public sealed class ShouldRelateToPropertyMembersThat<TNextElement, TRuleType>
        : AddPropertyMemberPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToPropertyMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TNextElement CreateNextElement(IPredicate<PropertyMember> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TNextElement, TRuleType>(_ruleCreator);
        }
    }
}
