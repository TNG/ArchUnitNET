using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class
        AttributesShouldConjunctionWithDescription : ObjectsShouldConjunctionWithDescription<AttributesShould, Attribute
        >
    {
        public AttributesShouldConjunctionWithDescription(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}