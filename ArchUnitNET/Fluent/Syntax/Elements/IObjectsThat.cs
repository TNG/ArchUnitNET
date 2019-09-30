using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TRuleTypeConjunction Are(IEnumerable<ICanBeAnalyzed> objects);
        TRuleTypeConjunction DependOn(string pattern);
        TRuleTypeConjunction DependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction DependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DependOn(IObjectProvider<IType> types);
        TRuleTypeConjunction DependOn(IEnumerable<IType> types);
        TRuleTypeConjunction DependOn(IEnumerable<Type> types);
        TRuleTypeConjunction OnlyDependOn(string pattern);
        TRuleTypeConjunction OnlyDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction OnlyDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction OnlyDependOn(IObjectProvider<IType> types);
        TRuleTypeConjunction OnlyDependOn(IEnumerable<IType> types);
        TRuleTypeConjunction OnlyDependOn(IEnumerable<Type> types);
        TRuleTypeConjunction HaveName(string name);
        TRuleTypeConjunction HaveFullName(string fullname);
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
        TRuleTypeConjunction DoNotDependOn(string pattern);
        TRuleTypeConjunction DoNotDependOn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction DoNotDependOn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DoNotDependOn(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotDependOn(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotDependOn(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveName(string name);
        TRuleTypeConjunction DoNotHaveFullName(string fullname);
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