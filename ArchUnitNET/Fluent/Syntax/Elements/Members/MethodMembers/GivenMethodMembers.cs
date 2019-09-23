using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembers : GivenObjects<GivenMethodMembersThat, MethodMembersShould, MethodMember>
    {
        public GivenMethodMembers(IArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}