using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
    public sealed class ShouldRelateToFieldMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : AddFieldMemberPredicate<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToFieldMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TRuleTypeShouldConjunction CreateNextElement(
            IPredicate<FieldMember> predicate
        )
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
