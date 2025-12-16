using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public sealed class MembersShould : AddMemberCondition<IMember, MembersShouldConjunction>
    {
        public MembersShould(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }

        protected override MembersShouldConjunction CreateNextElement(
            IOrderedCondition<IMember> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new MembersShouldConjunction(_ruleCreator);
        }
    }
}
