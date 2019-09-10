using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributes : GivenObjects<GivenAttributesThat, AttributesShould, Attribute>
    {
        public GivenAttributes(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}