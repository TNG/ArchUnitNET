using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunctionWithReason : GivenObjectsConjunctionWithReason<
        GivenMembersThat<GivenMembersConjunction, IMember>,
        MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public GivenMembersConjunctionWithReason(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}