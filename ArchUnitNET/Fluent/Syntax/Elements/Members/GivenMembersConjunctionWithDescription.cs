using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenMembersThat, MembersShould, IMember>
    {
        public GivenMembersConjunctionWithDescription(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
