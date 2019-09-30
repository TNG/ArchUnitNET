using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface
        ITypesShould<TRuleTypeShouldConjunction, TRuleType> : IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction Be(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction ImplementInterface(string pattern);
        TRuleTypeShouldConjunction ImplementInterface(Interface intf);
        TRuleTypeShouldConjunction ResideInNamespace(string pattern);
        TRuleTypeShouldConjunction HavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction HaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMemberWithName(string name);
        TRuleTypeShouldConjunction BeNested();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat();


        //Negations


        TRuleTypeShouldConjunction NotBe(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotImplementInterface(string pattern);
        TRuleTypeShouldConjunction NotImplementInterface(Interface intf);
        TRuleTypeShouldConjunction NotResideInNamespace(string pattern);
        TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMemberWithName(string name);
        TRuleTypeShouldConjunction NotBeNested();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat();
    }
}