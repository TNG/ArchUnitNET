using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        GivenFieldMembersConjunctionWithDescription : GivenObjectsConjunctionWithDescription<GivenFieldMembersThat,
            FieldMembersShould, FieldMember>
    {
        public GivenFieldMembersConjunctionWithDescription(IArchRuleCreator<FieldMember> ruleCreator) : base(
            ruleCreator)
        {
        }
    }
}