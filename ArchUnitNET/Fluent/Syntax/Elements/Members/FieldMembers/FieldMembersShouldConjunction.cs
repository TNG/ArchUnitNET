using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldConjunction : ObjectsShouldConjunction<FieldMembersShould,
        FieldMembersShouldConjunctionWithoutBecause, FieldMember>
    {
        public FieldMembersShouldConjunction(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}