using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public sealed class GivenMethodMembersThat
        : AddMethodMemberPredicate<GivenMethodMembersConjunction, MethodMember>
    {
        public GivenMethodMembersThat(IArchRuleCreator<MethodMember> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenMethodMembersConjunction CreateNextElement(
            IPredicate<MethodMember> predicate
        )
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenMethodMembersConjunction(_ruleCreator);
        }
    }
}
