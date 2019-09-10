using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldConjunction : ObjectsShouldConjunction<FieldMembersShould, FieldMember>
    {
        public FieldMembersShouldConjunction(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}