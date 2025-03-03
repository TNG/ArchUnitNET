using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenAttributesThat, AttributesShould, Attribute>
    {
        public GivenAttributesConjunctionWithDescription(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }
    }
}
