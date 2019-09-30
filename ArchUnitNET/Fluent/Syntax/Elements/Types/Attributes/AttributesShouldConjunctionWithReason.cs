using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class
        AttributesShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<AttributesShould, Attribute>
    {
        public AttributesShouldConjunctionWithReason(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}