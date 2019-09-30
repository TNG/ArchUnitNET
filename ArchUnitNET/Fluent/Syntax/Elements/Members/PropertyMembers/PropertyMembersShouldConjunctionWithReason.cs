using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<PropertyMembersShould,
            PropertyMember>
    {
        public PropertyMembersShouldConjunctionWithReason(IArchRuleCreator<PropertyMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}