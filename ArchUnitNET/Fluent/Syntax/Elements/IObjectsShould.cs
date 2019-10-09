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
        TRuleTypeShouldConjunction DependOnAnyTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction DependOnAny(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction DependOnAny(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types);
        TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types);
        TRuleTypeShouldConjunction OnlyDependOnTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types);
        TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types);
        TRuleTypeShouldConjunction HaveName(string name);
        TRuleTypeShouldConjunction HaveNameMatching(string pattern);
        TRuleTypeShouldConjunction HaveFullName(string fullname);
        TRuleTypeShouldConjunction HaveFullNameMatching(string pattern);
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

        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyClassesThat();
        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyInterfacesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnAnyTypesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat();


        //Negations

        TRuleTypeShouldConjunction NotExist();
        TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeShouldConjunction NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeShouldConjunction NotDependOnAnyTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotDependOnAny(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction NotDependOnAny(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotDependOnAny(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<IType> types);
        TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotHaveName(string name);
        TRuleTypeShouldConjunction NotHaveNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHaveFullName(string fullname);
        TRuleTypeShouldConjunction NotHaveFullNameMatching(string pattern);
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

        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnAnyTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat();
    }
}