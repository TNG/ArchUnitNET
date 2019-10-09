using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShould : MembersShould<FieldMembersShouldConjunction, FieldMember>,
        IFieldMemberConditions
    {
        public FieldMembersShould(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}