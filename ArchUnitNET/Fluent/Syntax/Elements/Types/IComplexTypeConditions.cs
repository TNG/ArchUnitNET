using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface
        IComplexTypeConditions<TRuleTypeShouldConjunction, TRuleType> :
            IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>, ITypeConditions<TRuleTypeShouldConjunction>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
    }
}