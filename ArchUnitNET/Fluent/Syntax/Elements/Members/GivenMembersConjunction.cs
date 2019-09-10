using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunction : GivenObjectsConjunction<GivenMembersThat<GivenMembersConjunction, IMember>,
        MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public GivenMembersConjunction(ArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}