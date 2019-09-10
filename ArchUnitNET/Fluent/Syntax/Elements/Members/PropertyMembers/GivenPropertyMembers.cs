using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembers : GivenObjects<GivenPropertyMembersThat, PropertyMembersShould, PropertyMember>
    {
        public GivenPropertyMembers(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}