using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class GivenFieldMembersThat
        : AddFieldMemberPredicate<FieldMember, GivenFieldMembersConjunction>
    {
        public GivenFieldMembersThat(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenFieldMembersConjunction CreateNextElement(
            IPredicate<FieldMember> predicate
        )
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenFieldMembersConjunction(_ruleCreator);
        }
    }
}
