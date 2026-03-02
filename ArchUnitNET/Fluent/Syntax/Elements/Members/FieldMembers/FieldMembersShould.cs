using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class FieldMembersShould : AddFieldMemberCondition<FieldMembersShouldConjunction>
    {
        public FieldMembersShould(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator) { }

        protected override FieldMembersShouldConjunction CreateNextElement(
            IOrderedCondition<FieldMember> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new FieldMembersShouldConjunction(_ruleCreator);
        }
    }
}
