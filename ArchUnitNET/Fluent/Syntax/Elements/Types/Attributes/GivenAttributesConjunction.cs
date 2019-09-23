using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunction : GivenObjectsConjunction<GivenAttributesThat, AttributesShould,
        GivenAttributesConjunctionWithoutBecause, Attribute>
    {
        public GivenAttributesConjunction(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}