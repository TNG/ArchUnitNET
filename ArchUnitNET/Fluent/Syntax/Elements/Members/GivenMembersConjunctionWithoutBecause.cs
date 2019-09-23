using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<
        GivenMembersThat<GivenMembersConjunction, IMember>,
        MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public GivenMembersConjunctionWithoutBecause(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}