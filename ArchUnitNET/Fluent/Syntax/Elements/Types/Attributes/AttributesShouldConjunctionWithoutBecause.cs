using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class
        AttributesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<AttributesShould, Attribute>
    {
        public AttributesShouldConjunctionWithoutBecause(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}