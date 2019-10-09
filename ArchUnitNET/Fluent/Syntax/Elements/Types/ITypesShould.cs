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
        TRuleTypeShouldConjunction BeAssignableToTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction BeAssignableTo(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction BeAssignableTo(Type type, params Type[] moreTypes);
        TRuleTypeShouldConjunction BeAssignableTo(ObjectProvider<IType> types);
        TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<IType> types);
        TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<Type> types);
        TRuleTypeShouldConjunction ImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction ResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction HaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction HaveMemberWithName(string name);
        TRuleTypeShouldConjunction BeNested();


        //Negations


        TRuleTypeShouldConjunction NotBe(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotBe(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotBeAssignableToTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotBeAssignableTo(IType type, params IType[] moreTypes);
        TRuleTypeShouldConjunction NotBeAssignableTo(Type type, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotBeAssignableTo(ObjectProvider<IType> types);
        TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<IType> types);
        TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name);
        TRuleTypeShouldConjunction NotHaveMemberWithName(string name);
        TRuleTypeShouldConjunction NotBeNested();
    }
}