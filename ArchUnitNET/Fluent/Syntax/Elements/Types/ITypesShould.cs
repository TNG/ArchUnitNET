using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface
        ITypesShould<TRuleTypeShouldConjunction, TRuleType> : IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction ImplementInterface(string pattern);
        TRuleTypeShouldConjunction ResideInNamespace(string pattern);
        TRuleTypeShouldConjunction HavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction HaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMemberWithName(string name);
        TRuleTypeShouldConjunction BeNested();


        //Negations


        TRuleTypeShouldConjunction NotImplementInterface(string pattern);
        TRuleTypeShouldConjunction NotResideInNamespace(string pattern);
        TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMemberWithName(string name);
        TRuleTypeShouldConjunction NotBeNested();
    }
}