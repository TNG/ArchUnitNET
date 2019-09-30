using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        GivenFieldMembersConjunctionWithReason : GivenObjectsConjunctionWithReason<GivenFieldMembersThat,
            FieldMembersShould, FieldMember>
    {
        public GivenFieldMembersConjunctionWithReason(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}