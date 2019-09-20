using ArchUnitNET.Domain;

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
            _ruleCreator.AddCondition(MethodMembersConditionDefinition.BeConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(MethodMembersConditionDefinition.BeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public MethodMembersShouldConjunction BeNoConstructor()
        {
            _ruleCreator.AddCondition(MethodMembersConditionDefinition.BeNoConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(MethodMembersConditionDefinition.NotBeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}