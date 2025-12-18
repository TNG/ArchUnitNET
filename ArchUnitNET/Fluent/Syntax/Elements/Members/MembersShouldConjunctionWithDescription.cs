using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<MembersShould, IMember>
    {
        public MembersShouldConjunctionWithDescription(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
