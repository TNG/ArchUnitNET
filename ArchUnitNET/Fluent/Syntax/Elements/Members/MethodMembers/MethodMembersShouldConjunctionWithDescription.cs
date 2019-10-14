using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        MethodMembersShouldConjunctionWithDescription : ObjectsShouldConjunctionWithDescription<MethodMembersShould,
            MethodMember>
    {
        public MethodMembersShouldConjunctionWithDescription(IArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}