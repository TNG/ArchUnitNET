using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        GivenMethodMembersConjunctionWithReason : GivenObjectsConjunctionWithReason<GivenMethodMembersThat,
            MethodMembersShould, MethodMember>
    {
        public GivenMethodMembersConjunctionWithReason(IArchRuleCreator<MethodMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}