using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        MethodMembersShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<MethodMembersShould,
            MethodMember>
    {
        public MethodMembersShouldConjunctionWithReason(IArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}