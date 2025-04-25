using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembers
        : GivenObjects<GivenPropertyMembersThat, PropertyMembersShould, PropertyMember>
    {
        public GivenPropertyMembers(IArchRuleCreator<PropertyMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
