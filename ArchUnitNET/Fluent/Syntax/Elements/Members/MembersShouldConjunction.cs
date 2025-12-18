using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldConjunction
        : ObjectsShouldConjunction<MembersShould, MembersShouldConjunctionWithDescription, IMember>
    {
        public MembersShouldConjunction(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
