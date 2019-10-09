using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeConjunction Are(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeConjunction DependOnAnyTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DependOnAny(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction DependOnAny(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DependOnAny(IObjectProvider<IType> types);
        TRuleTypeConjunction DependOnAny(IEnumerable<IType> types);
        TRuleTypeConjunction DependOnAny(IEnumerable<Type> types);
        TRuleTypeConjunction OnlyDependOnTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction OnlyDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction OnlyDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction OnlyDependOn(IObjectProvider<IType> types);
        TRuleTypeConjunction OnlyDependOn(IEnumerable<IType> types);
        TRuleTypeConjunction OnlyDependOn(IEnumerable<Type> types);
        TRuleTypeConjunction HaveName(string name);
        TRuleTypeConjunction HaveNameMatching(string pattern);
        TRuleTypeConjunction HaveFullName(string fullname);
        TRuleTypeConjunction HaveFullNameMatching(string pattern);
        TRuleTypeConjunction HaveNameStartingWith(string pattern);
        TRuleTypeConjunction HaveNameEndingWith(string pattern);
        TRuleTypeConjunction HaveNameContaining(string pattern);
        TRuleTypeConjunction ArePrivate();
        TRuleTypeConjunction ArePublic();
        TRuleTypeConjunction AreProtected();
        TRuleTypeConjunction AreInternal();
        TRuleTypeConjunction AreProtectedInternal();
        TRuleTypeConjunction ArePrivateProtected();


        //Negations


        TRuleTypeConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeConjunction DoNotDependOnAnyTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotDependOnAny(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction DoNotDependOnAny(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DoNotDependOnAny(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotDependOnAny(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotDependOnAny(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveName(string name);
        TRuleTypeConjunction DoNotHaveNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveFullName(string fullname);
        TRuleTypeConjunction DoNotHaveFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveNameStartingWith(string pattern);
        TRuleTypeConjunction DoNotHaveNameEndingWith(string pattern);
        TRuleTypeConjunction DoNotHaveNameContaining(string pattern);
        TRuleTypeConjunction AreNotPrivate();
        TRuleTypeConjunction AreNotPublic();
        TRuleTypeConjunction AreNotProtected();
        TRuleTypeConjunction AreNotInternal();
        TRuleTypeConjunction AreNotProtectedInternal();
        TRuleTypeConjunction AreNotPrivateProtected();
    }
}