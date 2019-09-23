using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<GivenAttributesThat,
        AttributesShould, Attribute>
    {
        public GivenAttributesConjunctionWithoutBecause(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}