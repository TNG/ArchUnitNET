using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunction : GivenObjectsConjunction<GivenPropertyMembersThat,
        PropertyMembersShould, GivenPropertyMembersConjunctionWithoutBecause, PropertyMember>
    {
        public GivenPropertyMembersConjunction(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}