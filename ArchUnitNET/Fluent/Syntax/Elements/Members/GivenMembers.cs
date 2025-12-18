using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembers : GivenObjects<GivenMembersThat, MembersShould, IMember>
    {
        public GivenMembers(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
