using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShould
        : MembersShould<FieldMembersShouldConjunction, FieldMember>,
            IComplexFieldMemberConditions
    {
        public FieldMembersShould(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
