using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        GivenMethodMembersConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<GivenMethodMembersThat,
            MethodMembersShould, MethodMember>
    {
        public GivenMethodMembersConjunctionWithoutBecause(ArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}