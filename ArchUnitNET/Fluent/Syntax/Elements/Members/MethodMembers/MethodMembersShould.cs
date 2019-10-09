using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShould : MembersShould<MethodMembersShouldConjunction, MethodMember>,
        IComplexMethodMemberConditions
    {
        public MethodMembersShould(IArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public MethodMembersShouldConjunction BeConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public MethodMembersShouldConjunction BeNoConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeNoConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}