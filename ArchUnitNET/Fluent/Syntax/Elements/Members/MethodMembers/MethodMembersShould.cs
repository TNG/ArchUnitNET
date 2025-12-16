using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShould : AddMethodMemberCondition<MethodMembersShouldConjunction>
    {
        public MethodMembersShould(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }

        protected override MethodMembersShouldConjunction CreateNextElement(
            IOrderedCondition<MethodMember> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}
