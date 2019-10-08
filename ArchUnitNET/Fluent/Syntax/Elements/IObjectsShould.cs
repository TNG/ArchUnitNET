using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction Exist();
        TRuleTypeShouldConjunction Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeShouldConjunction Be(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeShouldConjunction DependOnTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction DependOn(IType type);
        TRuleTypeShouldConjunction DependOn(Type type);
        TRuleTypeShouldConjunction OnlyDependOnTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types);
        TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types);
        TRuleTypeShouldConjunction HaveName(string name);
        TRuleTypeShouldConjunction HaveFullName(string fullname);
        TRuleTypeShouldConjunction HaveNameStartingWith(string pattern);
        TRuleTypeShouldConjunction HaveNameEndingWith(string pattern);
        TRuleTypeShouldConjunction HaveNameContaining(string pattern);
        TRuleTypeShouldConjunction BePrivate();
        TRuleTypeShouldConjunction BePublic();
        TRuleTypeShouldConjunction BeProtected();
        TRuleTypeShouldConjunction BeInternal();
        TRuleTypeShouldConjunction BeProtectedInternal();
        TRuleTypeShouldConjunction BePrivateProtected();

        //Relation Conditions

        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> DependOnClassesThat();
        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> DependOnInterfacesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnTypesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat();


        //Negations

        TRuleTypeShouldConjunction NotExist();
        TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeShouldConjunction NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeShouldConjunction NotDependOnTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction NotDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotDependOn(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction NotDependOn(IEnumerable<IType> types);
        TRuleTypeShouldConjunction NotDependOn(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotHaveName(string name);
        TRuleTypeShouldConjunction NotHaveFullName(string fullname);
        TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern);
        TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern);
        TRuleTypeShouldConjunction NotHaveNameContaining(string pattern);
        TRuleTypeShouldConjunction NotBePrivate();
        TRuleTypeShouldConjunction NotBePublic();
        TRuleTypeShouldConjunction NotBeProtected();
        TRuleTypeShouldConjunction NotBeInternal();
        TRuleTypeShouldConjunction NotBeProtectedInternal();
        TRuleTypeShouldConjunction NotBePrivateProtected();

        //Relation Condition Negations

        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat();
    }
}