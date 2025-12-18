using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class FieldMembersShould : AddFieldMemberCondition<FieldMembersShould>
    {
        public FieldMembersShould(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator) { }

        protected override FieldMembersShould CreateNextElement(
            IOrderedCondition<FieldMember> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new FieldMembersShould(_ruleCreator);
        }
    }
}
