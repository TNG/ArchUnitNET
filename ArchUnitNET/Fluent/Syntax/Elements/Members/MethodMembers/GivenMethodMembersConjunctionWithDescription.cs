using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenMethodMembersThat,
            MethodMembersShould,
            MethodMember
        >
    {
        public GivenMethodMembersConjunctionWithDescription(
            IArchRuleCreator<MethodMember> ruleCreator
        )
            : base(ruleCreator) { }
    }
}
