using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunction
        : GivenObjectsConjunction<
            GivenMembersThat,
            MembersShould,
            GivenMembersConjunctionWithDescription,
            IMember
        >
    {
        public GivenMembersConjunction(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
