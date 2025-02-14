using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersConjunction
        : GivenObjectsConjunction<
            GivenMethodMembersThat,
            MethodMembersShould,
            GivenMethodMembersConjunctionWithDescription,
            MethodMember
        >
    {
        public GivenMethodMembersConjunction(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
