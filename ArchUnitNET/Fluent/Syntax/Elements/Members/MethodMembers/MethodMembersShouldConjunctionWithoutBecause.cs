using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        MethodMembersShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<MethodMembersShould,
            MethodMember>
    {
        public MethodMembersShouldConjunctionWithoutBecause(ArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}