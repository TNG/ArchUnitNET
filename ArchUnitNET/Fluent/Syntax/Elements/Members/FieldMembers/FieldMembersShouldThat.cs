using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        MembersShouldThat<TRuleTypeShouldConjunction, FieldMember, TRuleType>,
        IFieldMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public FieldMembersShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.FieldMembers)
        {
        }
    }
}