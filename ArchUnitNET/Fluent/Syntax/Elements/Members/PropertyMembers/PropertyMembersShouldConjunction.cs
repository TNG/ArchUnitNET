using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunction : ObjectsShouldConjunction<PropertyMembersShould, PropertyMember>
    {
        public PropertyMembersShouldConjunction(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}