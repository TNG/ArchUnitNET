using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunctionWithReason : GivenObjectsConjunctionWithReason<
        GivenPropertyMembersThat,
        PropertyMembersShould, PropertyMember>
    {
        public GivenPropertyMembersConjunctionWithReason(IArchRuleCreator<PropertyMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}