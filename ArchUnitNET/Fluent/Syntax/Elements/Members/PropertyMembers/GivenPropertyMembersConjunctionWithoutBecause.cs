using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<
        GivenPropertyMembersThat,
        PropertyMembersShould, PropertyMember>
    {
        public GivenPropertyMembersConjunctionWithoutBecause(IArchRuleCreator<PropertyMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}