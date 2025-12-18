using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public sealed class GivenAttributesThat
        : AddAttributePredicate<GivenAttributesConjunction, Attribute>
    {
        public GivenAttributesThat(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenAttributesConjunction CreateNextElement(
            IPredicate<Attribute> predicate
        )
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenAttributesConjunction(_ruleCreator);
        }
    }
}
