using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class
        GivenFieldMembersConjunction : GivenObjectsConjunction<GivenFieldMembersThat, FieldMembersShould,
            GivenFieldMembersConjunctionWithDescription, FieldMember>
    {
        public GivenFieldMembersConjunction(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}