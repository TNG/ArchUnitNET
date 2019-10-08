using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface
        ITypesShould<TRuleTypeShouldConjunction, TRuleType> : IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction Be(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction Be(IEnumerable<Type> types);
        TRuleTypeShouldConjunction ImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction ImplementInterface(Interface intf);
        TRuleTypeShouldConjunction ResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction HaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMemberWithName(string name);
        TRuleTypeShouldConjunction BeNested();


        //Negations


        TRuleTypeShouldConjunction NotBe(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotBe(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotImplementInterface(Interface intf);
        TRuleTypeShouldConjunction NotResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMemberWithName(string name);
        TRuleTypeShouldConjunction NotBeNested();
    }
}