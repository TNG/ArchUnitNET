using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class
        PropertyMembersShouldConjunction : ObjectsShouldConjunction<PropertyMembersShouldConjunction, PropertyMember>
    {
        public PropertyMembersShouldConjunction(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}