using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
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
