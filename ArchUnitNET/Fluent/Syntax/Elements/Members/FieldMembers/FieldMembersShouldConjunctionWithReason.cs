using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        FieldMembersShouldConjunctionWithReason :
            ObjectsShouldConjunctionWithReason<FieldMembersShould, FieldMember>
    {
        public FieldMembersShouldConjunctionWithReason(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator)
        {
        }
    }
}