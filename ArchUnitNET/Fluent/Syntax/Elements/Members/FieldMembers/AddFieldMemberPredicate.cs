using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public abstract class AddFieldMemberPredicate<TNextElement, TRelatedType>
        : AddMemberPredicate<TNextElement, TRelatedType, FieldMember>,
            IAddFieldMemberPredicate<TNextElement, FieldMember>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddFieldMemberPredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }
    }
}
