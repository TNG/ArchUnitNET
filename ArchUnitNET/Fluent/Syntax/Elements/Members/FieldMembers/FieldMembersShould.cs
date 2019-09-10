using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShould : MembersShould<FieldMembersShouldConjunction, FieldMember>,
        IFieldMembersShould
    {
        public FieldMembersShould(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}