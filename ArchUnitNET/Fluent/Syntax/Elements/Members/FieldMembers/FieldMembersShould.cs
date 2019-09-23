using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShould : MembersShould<FieldMembersShouldConjunction, FieldMember>,
        IFieldMembersShould
    {
        public FieldMembersShould(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}