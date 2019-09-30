using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunction : ObjectsShouldConjunction<PropertyMembersShould,
            PropertyMembersShouldConjunctionWithReason, PropertyMember>
    {
        public PropertyMembersShouldConjunction(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}