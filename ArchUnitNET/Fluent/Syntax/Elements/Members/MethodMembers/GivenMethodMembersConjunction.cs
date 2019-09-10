using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class
        GivenMethodMembersConjunction : GivenObjectsConjunction<GivenMethodMembersThat, MethodMembersShould,
            MethodMember>
    {
        public GivenMethodMembersConjunction(ArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}