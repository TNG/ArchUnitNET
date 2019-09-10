using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShould : TypesShould<AttributesShouldConjunction, Attribute>, IAttributesShould
    {
        public AttributesShould(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public AttributesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddSimpleCondition(attribute => attribute.IsAbstract);
            return new AttributesShouldConjunction(_ruleCreator);
        }


        //Negations


        public AttributesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddSimpleCondition(attribute => !attribute.IsAbstract);
            return new AttributesShouldConjunction(_ruleCreator);
        }
    }
}