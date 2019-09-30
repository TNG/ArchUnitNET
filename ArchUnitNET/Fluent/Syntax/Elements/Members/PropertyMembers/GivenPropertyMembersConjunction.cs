using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunction : GivenObjectsConjunction<GivenPropertyMembersThat,
        PropertyMembersShould, GivenPropertyMembersConjunctionWithReason, PropertyMember>
    {
        public GivenPropertyMembersConjunction(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}