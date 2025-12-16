using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public abstract class AddFieldMemberCondition<TNextElement>
        : AddMemberCondition<FieldMember, TNextElement>,
            IFieldMemberConditions<TNextElement, FieldMember>
    {
        internal AddFieldMemberCondition(IArchRuleCreator<FieldMember> ruleCreator)
            : base(ruleCreator) { }
    }
}
