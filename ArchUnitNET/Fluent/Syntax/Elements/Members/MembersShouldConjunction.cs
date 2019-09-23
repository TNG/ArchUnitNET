using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class
        MembersShouldConjunction : ObjectsShouldConjunction<MembersShould<MembersShouldConjunction, IMember>,
            MembersShouldConjunctionWithoutBecause, IMember>
    {
        public MembersShouldConjunction(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}