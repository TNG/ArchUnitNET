using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        GivenFieldMembersConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<GivenFieldMembersThat,
            FieldMembersShould, FieldMember>
    {
        public GivenFieldMembersConjunctionWithoutBecause(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}