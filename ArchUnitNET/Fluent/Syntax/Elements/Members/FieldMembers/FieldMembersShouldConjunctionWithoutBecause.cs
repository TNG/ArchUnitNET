using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        FieldMembersShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<FieldMembersShould,
            FieldMember>
    {
        public FieldMembersShouldConjunctionWithoutBecause(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}