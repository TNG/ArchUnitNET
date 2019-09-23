using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembers : GivenObjects<GivenFieldMembersThat, FieldMembersShould, FieldMember>
    {
        public GivenFieldMembers(IArchRuleCreator<FieldMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}