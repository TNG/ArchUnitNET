using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould
        : AddPropertyMemberCondition<PropertyMembersShouldConjunction>
    {
        public PropertyMembersShould(IArchRuleCreator<PropertyMember> ruleCreator)
            : base(ruleCreator) { }

        protected override PropertyMembersShouldConjunction CreateNextElement(
            IOrderedCondition<PropertyMember> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}
