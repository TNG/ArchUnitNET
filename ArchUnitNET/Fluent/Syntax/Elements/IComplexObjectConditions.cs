using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        : IObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > DependOnAnyTypesThat();
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > OnlyDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAnyAttributesThat();
        ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > OnlyHaveAttributesThat();

        //Negations

        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotDependOnAnyTypesThat();
        ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > NotHaveAnyAttributesThat();
    }
}
