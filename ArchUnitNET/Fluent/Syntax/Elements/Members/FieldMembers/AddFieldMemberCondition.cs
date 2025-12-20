using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public abstract class AddFieldMemberCondition<TNextElement>
        : AddMemberCondition<TNextElement, FieldMember>,
            IAddFieldMemberCondition<TNextElement, FieldMember>
    {
        internal AddFieldMemberCondition(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }
    }
}
