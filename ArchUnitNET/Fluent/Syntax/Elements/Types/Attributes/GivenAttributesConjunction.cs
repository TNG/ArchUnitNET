using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunction
        : GivenObjectsConjunction<
            GivenAttributesThat,
            AttributesShould,
            GivenAttributesConjunctionWithDescription,
            Attribute
        >
    {
        public GivenAttributesConjunction(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }
    }
}
