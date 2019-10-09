using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface
        IComplexMemberConditions<TRuleTypeShouldConjunction, TRuleType> :
            IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>,
            IMemberConditions<TRuleTypeShouldConjunction>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
    }
}