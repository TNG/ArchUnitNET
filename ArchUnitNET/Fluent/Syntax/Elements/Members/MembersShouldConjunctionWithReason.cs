using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class
        MembersShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<
            MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public MembersShouldConjunctionWithReason(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}