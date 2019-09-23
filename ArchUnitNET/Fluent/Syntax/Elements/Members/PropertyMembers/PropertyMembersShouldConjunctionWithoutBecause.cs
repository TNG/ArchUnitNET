using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<PropertyMembersShould,
            PropertyMember>
    {
        public PropertyMembersShouldConjunctionWithoutBecause(IArchRuleCreator<PropertyMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}