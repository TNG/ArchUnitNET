using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunction
        : GivenObjectsConjunction<
            GivenMembersThat<GivenMembersConjunction, IMember>,
            MembersShould<MembersShouldConjunction, IMember>,
            GivenMembersConjunctionWithDescription,
            IMember
        >
    {
        public GivenMembersConjunction(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
