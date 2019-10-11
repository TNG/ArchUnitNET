using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectPredicates<out TReturnType>
    {
        TReturnType Are(string pattern, bool useRegularExpressions = false);
        TReturnType Are(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Are(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Are(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType DependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);
        TReturnType HaveName(string name);
        TReturnType HaveNameMatching(string pattern);
        TReturnType HaveFullName(string fullname);
        TReturnType HaveFullNameMatching(string pattern);
        TReturnType HaveNameStartingWith(string pattern);
        TReturnType HaveNameEndingWith(string pattern);
        TReturnType HaveNameContaining(string pattern);
        TReturnType HaveFullNameContaining(string pattern);
        TReturnType ArePrivate();
        TReturnType ArePublic();
        TReturnType AreProtected();
        TReturnType AreInternal();
        TReturnType AreProtectedInternal();
        TReturnType ArePrivateProtected();


        //Negations


        TReturnType AreNot(string pattern, bool useRegularExpressions = false);
        TReturnType AreNot(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType AreNot(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType AreNot(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType DoNotDependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotDependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DoNotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DoNotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DoNotDependOnAny(IObjectProvider<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<Type> types);
        TReturnType DoNotHaveName(string name);
        TReturnType DoNotHaveNameMatching(string pattern);
        TReturnType DoNotHaveFullName(string fullname);
        TReturnType DoNotHaveFullNameMatching(string pattern);
        TReturnType DoNotHaveNameStartingWith(string pattern);
        TReturnType DoNotHaveNameEndingWith(string pattern);
        TReturnType DoNotHaveNameContaining(string pattern);
        TReturnType DoNotHaveFullNameContaining(string pattern);
        TReturnType AreNotPrivate();
        TReturnType AreNotPublic();
        TReturnType AreNotProtected();
        TReturnType AreNotInternal();
        TReturnType AreNotProtectedInternal();
        TReturnType AreNotPrivateProtected();
    }
}