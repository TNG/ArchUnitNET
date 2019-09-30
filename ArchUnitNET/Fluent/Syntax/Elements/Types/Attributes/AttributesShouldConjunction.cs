using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldConjunction : ObjectsShouldConjunction<AttributesShould,
        AttributesShouldConjunctionWithReason, Attribute>
    {
        public AttributesShouldConjunction(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}