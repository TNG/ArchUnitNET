using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class
        MembersShouldConjunction : ObjectsShouldConjunction<MembersShould<MembersShouldConjunction, IMember>,
            MembersShouldConjunctionWithReason, IMember>
    {
        public MembersShouldConjunction(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}