using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface IComplexTypeConditions<TRuleTypeShouldConjunction, TRuleType>
        : IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>,
            ITypeConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > BeAssignableToTypesThat();

        //Negations

        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotBeAssignableToTypesThat();
    }
}
