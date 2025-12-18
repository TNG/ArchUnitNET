using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public sealed class GivenPropertyMembersThat
        : AddPropertyMemberPredicate<GivenPropertyMembersConjunction, PropertyMember>
    {
        public GivenPropertyMembersThat(IArchRuleCreator<PropertyMember> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenPropertyMembersConjunction CreateNextElement(
            IPredicate<PropertyMember> predicate
        )
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}
