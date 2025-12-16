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
        // csharpier-ignore-start
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyTypesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnTypesThat();

        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAnyAttributesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat();

        // Negations

        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAnyAttributesThat();
        // csharpier-ignore-end
    }
}
