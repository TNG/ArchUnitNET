using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        FieldMembersShouldConjunctionWithDescription :
            ObjectsShouldConjunctionWithDescription<FieldMembersShould, FieldMember>
    {
        public FieldMembersShouldConjunctionWithDescription(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator)
        {
        }
    }
}