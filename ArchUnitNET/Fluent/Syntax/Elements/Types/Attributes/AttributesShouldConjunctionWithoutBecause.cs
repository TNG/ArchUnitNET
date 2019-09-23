using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class
        AttributesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<AttributesShould, Attribute>
    {
        public AttributesShouldConjunctionWithoutBecause(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}