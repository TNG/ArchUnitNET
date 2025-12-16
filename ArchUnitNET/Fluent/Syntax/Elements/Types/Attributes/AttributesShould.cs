using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShould
        : AddAttributeCondition<AttributesShouldConjunction>
    {
        public AttributesShould(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }

        protected override AttributesShouldConjunction CreateNextElement(IOrderedCondition<Attribute> condition)
        {
            _ruleCreator.AddCondition(condition);
            return new AttributesShouldConjunction(_ruleCreator);
        }
    }
}
