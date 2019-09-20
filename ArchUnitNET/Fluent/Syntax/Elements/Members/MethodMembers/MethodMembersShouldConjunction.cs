using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldConjunction : ObjectsShouldConjunction<MethodMembersShould,
        MethodMembersShouldConjunctionWithoutBecause, MethodMember>
    {
        public MethodMembersShouldConjunction(ArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}