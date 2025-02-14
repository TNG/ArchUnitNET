using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldConjunction
        : ObjectsShouldConjunction<
            MethodMembersShould,
            MethodMembersShouldConjunctionWithDescription,
            MethodMember
        >
    {
        public MethodMembersShouldConjunction(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
