using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

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

        ShouldRelateToInterfacesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > ImplementAnyInterfacesThat();

        //Negations

        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotBeAssignableToTypesThat();

        ShouldRelateToInterfacesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > NotImplementAnyInterfacesThat();
    }
}
