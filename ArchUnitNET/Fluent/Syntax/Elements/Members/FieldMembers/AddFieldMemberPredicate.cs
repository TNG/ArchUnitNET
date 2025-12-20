using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public abstract class AddFieldMemberPredicate<TNextElement>
        : AddMemberPredicate<TNextElement, FieldMember>,
            IAddFieldMemberPredicate<TNextElement, FieldMember>
    {
        internal AddFieldMemberPredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }
    }
}
