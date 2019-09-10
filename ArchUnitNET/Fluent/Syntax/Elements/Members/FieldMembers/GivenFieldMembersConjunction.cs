using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        GivenFieldMembersConjunction : GivenObjectsConjunction<GivenFieldMembersThat, FieldMembersShould, FieldMember>
    {
        public GivenFieldMembersConjunction(ArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}