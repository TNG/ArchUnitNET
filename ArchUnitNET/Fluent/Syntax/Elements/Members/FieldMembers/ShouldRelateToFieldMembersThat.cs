using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class ShouldRelateToFieldMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : AddMemberPredicate<FieldMember, TRuleType, TRuleTypeShouldConjunction>
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
