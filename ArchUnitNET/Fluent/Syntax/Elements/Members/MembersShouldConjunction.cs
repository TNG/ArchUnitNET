using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldConjunction
        : ObjectsShouldConjunction<
            MembersShould<MembersShouldConjunction, IMember>,
            MembersShouldConjunctionWithDescription,
            IMember
        >
    {
        public MembersShouldConjunction(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
