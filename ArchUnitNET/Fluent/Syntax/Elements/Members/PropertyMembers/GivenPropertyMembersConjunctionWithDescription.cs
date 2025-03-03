using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenPropertyMembersThat,
            PropertyMembersShould,
            PropertyMember
        >
    {
        public GivenPropertyMembersConjunctionWithDescription(
            IArchRuleCreator<PropertyMember> ruleCreator
        )
            : base(ruleCreator) { }
    }
}
