using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShould : TypesShould<AttributesShouldConjunction, Attribute>, IComplexAttributeConditions
    {
        public AttributesShould(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public AttributesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddCondition(AttributeConditionsDefinition.BeAbstract());
            return new AttributesShouldConjunction(_ruleCreator);
        }

        public AttributesShouldConjunction BeSealed()
        {
            _ruleCreator.AddCondition(AttributeConditionsDefinition.BeSealed());
            return new AttributesShouldConjunction(_ruleCreator);
        }


        //Negations


        public AttributesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddCondition(AttributeConditionsDefinition.NotBeAbstract());
            return new AttributesShouldConjunction(_ruleCreator);
        }

        public AttributesShouldConjunction NotBeSealed()
        {
            _ruleCreator.AddCondition(AttributeConditionsDefinition.NotBeSealed());
            return new AttributesShouldConjunction(_ruleCreator);
        }
    }
}