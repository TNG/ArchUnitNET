using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunctionWithDescription : ObjectsShouldConjunctionWithDescription<PropertyMembersShould,
            PropertyMember>
    {
        public PropertyMembersShouldConjunctionWithDescription(IArchRuleCreator<PropertyMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}