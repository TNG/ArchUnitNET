using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembersThat : GivenMembersThat<GivenFieldMembersConjunction, FieldMember>,
        IFieldMemberPredicates<GivenFieldMembersConjunction>
    {
        public GivenFieldMembersThat(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}