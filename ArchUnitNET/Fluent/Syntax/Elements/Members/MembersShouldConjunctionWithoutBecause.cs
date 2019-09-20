using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class
        MembersShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<
            MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public MembersShouldConjunctionWithoutBecause(ArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}