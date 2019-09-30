using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunctionWithReason : GivenObjectsConjunctionWithReason<GivenAttributesThat,
        AttributesShould, Attribute>
    {
        public GivenAttributesConjunctionWithReason(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}