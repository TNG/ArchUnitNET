using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public sealed class ShouldRelateToMethodMembersThat<TNextElement, TRuleType>
        : AddMethodMemberPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToMethodMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TNextElement CreateNextElement(IPredicate<MethodMember> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TNextElement, TRuleType>(_ruleCreator);
        }
    }
}
