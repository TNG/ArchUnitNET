using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        MethodMembersShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<MethodMembersShould,
            MethodMember>
    {
        public MethodMembersShouldConjunctionWithoutBecause(IArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}