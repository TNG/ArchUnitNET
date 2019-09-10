using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembersThat : GivenMembersThat<GivenFieldMembersConjunction, FieldMember>,
        IFieldMembersThat<GivenFieldMembersConjunction>
    {
        public GivenFieldMembersThat(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}