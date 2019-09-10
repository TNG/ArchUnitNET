using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShould : MembersShould<MethodMembersShouldConjunction, MethodMember>,
        IMethodMembersShould
    {
        public MethodMembersShould(ArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public MethodMembersShouldConjunction BeConstructor()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsVirtual);
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        //Negations


        public MethodMembersShouldConjunction BeNoConstructor()
        {
            _ruleCreator.AddSimpleCondition(member => !member.IsConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => !member.IsVirtual);
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}