using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public abstract class AddFieldMemberPredicate<TRelatedType, TNextElement>
        : AddMemberPredicate<FieldMember, TRelatedType, TNextElement>,
            IFieldMemberPredicates<TNextElement, FieldMember>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddFieldMemberPredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }
    }
}
