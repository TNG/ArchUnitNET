using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunction : GivenObjectsConjunction<GivenPropertyMembersThat,
        PropertyMembersShould, PropertyMember>
    {
        public GivenPropertyMembersConjunction(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}