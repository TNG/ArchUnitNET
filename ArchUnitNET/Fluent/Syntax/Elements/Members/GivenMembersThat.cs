using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public sealed class GivenMembersThat
        : AddMemberPredicate<IMember, IMember, GivenMembersConjunction>
    {
        public GivenMembersThat(IArchRuleCreator<IMember> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenMembersConjunction CreateNextElement(IPredicate<IMember> predicate)
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenMembersConjunction(_ruleCreator);
        }
    }
}
