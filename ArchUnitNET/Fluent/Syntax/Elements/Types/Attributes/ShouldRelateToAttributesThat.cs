using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public sealed class ShouldRelateToAttributesThat<TNextElement, TRuleType>
        : AddAttributePredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToAttributesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TNextElement CreateNextElement(IPredicate<Attribute> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TNextElement, TRuleType>(_ruleCreator);
        }
    }
}
