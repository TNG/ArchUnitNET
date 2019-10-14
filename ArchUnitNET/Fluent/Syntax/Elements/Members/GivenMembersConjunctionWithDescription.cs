using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunctionWithDescription : GivenObjectsConjunctionWithDescription<
        GivenMembersThat<GivenMembersConjunction, IMember>,
        MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public GivenMembersConjunctionWithDescription(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}