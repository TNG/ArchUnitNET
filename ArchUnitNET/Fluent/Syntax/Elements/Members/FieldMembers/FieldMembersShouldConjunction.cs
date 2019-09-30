using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldConjunction : ObjectsShouldConjunction<FieldMembersShould,
        FieldMembersShouldConjunctionWithReason, FieldMember>
    {
        public FieldMembersShouldConjunction(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}