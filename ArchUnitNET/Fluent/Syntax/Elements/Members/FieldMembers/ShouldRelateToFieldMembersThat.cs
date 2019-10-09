using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class ShouldRelateToFieldMembersThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToMembersThat<TRuleTypeShouldConjunction, FieldMember, TRuleType>,
        IFieldMemberPredicates<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToFieldMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}