using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembers : GivenObjects<GivenMethodMembersThat, MethodMembersShould, MethodMember>
    {
        public GivenMethodMembers(ArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}