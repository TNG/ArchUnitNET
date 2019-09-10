using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction DependOn(string pattern);
        TRuleTypeShouldConjunction HaveNameStartingWith(string pattern);
        TRuleTypeShouldConjunction HaveNameEndingWith(string pattern);
        TRuleTypeShouldConjunction HaveNameContaining(string pattern);
        TRuleTypeShouldConjunction BePrivate();
        TRuleTypeShouldConjunction BePublic();
        TRuleTypeShouldConjunction BeProtected();
        TRuleTypeShouldConjunction BeInternal();
        TRuleTypeShouldConjunction BeProtectedInternal();
        TRuleTypeShouldConjunction BePrivateProtected();
        ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat();
        AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat();


        //Negations


        TRuleTypeShouldConjunction NotDependOn(string pattern);
        TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern);
        TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern);
        TRuleTypeShouldConjunction NotHaveNameContaining(string pattern);
        TRuleTypeShouldConjunction NotBePrivate();
        TRuleTypeShouldConjunction NotBePublic();
        TRuleTypeShouldConjunction NotBeProtected();
        TRuleTypeShouldConjunction NotBeInternal();
        TRuleTypeShouldConjunction NotBeProtectedInternal();
        TRuleTypeShouldConjunction NotBePrivateProtected();
        ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat();
        AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat();
    }
}