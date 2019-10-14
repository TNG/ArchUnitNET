using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class
        MembersShouldConjunctionWithDescription : ObjectsShouldConjunctionWithDescription<
            MembersShould<MembersShouldConjunction, IMember>, IMember>
    {
        public MembersShouldConjunctionWithDescription(IArchRuleCreator<IMember> ruleCreator) : base(ruleCreator)
        {
        }
    }
}