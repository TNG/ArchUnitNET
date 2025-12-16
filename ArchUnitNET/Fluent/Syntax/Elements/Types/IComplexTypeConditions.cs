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
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType> BeAssignableToTypesThat();

        ShouldRelateToInterfacesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > ImplementAnyInterfacesThat();

        //Negations

        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType> NotBeAssignableToTypesThat();

        ShouldRelateToInterfacesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > NotImplementAnyInterfacesThat();
    }
}
