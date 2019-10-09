using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShould : TypesShould<AttributesShouldConjunction, Attribute>, IAttributeConditions
    {
        public AttributesShould(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public AttributesShouldConjunction BeAbstract()
        {
            _ruleCreator.AddCondition(AttributesConditionDefinition.BeAbstract());
            return new AttributesShouldConjunction(_ruleCreator);
        }

        public AttributesShouldConjunction BeSealed()
        {
            _ruleCreator.AddCondition(AttributesConditionDefinition.BeSealed());
            return new AttributesShouldConjunction(_ruleCreator);
        }


        //Negations


        public AttributesShouldConjunction NotBeAbstract()
        {
            _ruleCreator.AddCondition(AttributesConditionDefinition.NotBeAbstract());
            return new AttributesShouldConjunction(_ruleCreator);
        }

        public AttributesShouldConjunction NotBeSealed()
        {
            _ruleCreator.AddCondition(AttributesConditionDefinition.NotBeSealed());
            return new AttributesShouldConjunction(_ruleCreator);
        }
    }
}