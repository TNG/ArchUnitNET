using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public sealed class ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>
        : AddTypePredicate<Attribute, TRuleType, TRuleTypeShouldConjunction>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToAttributesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TRuleTypeShouldConjunction CreateNextElement(
            IPredicate<Attribute> predicate
        )
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
