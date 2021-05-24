﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectPredicates<out TReturnType, TRuleType> where TRuleType : ICanBeAnalyzed
    {
        TReturnType Are(string pattern, bool useRegularExpressions = false);
        TReturnType Are(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Are(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Are(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType CallAny(string pattern, bool useRegularExpressions = false);
        TReturnType CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType CallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);
        TReturnType DependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType FollowCustomPredicate(IPredicate<TRuleType> predicate);
        TReturnType FollowCustomPredicate(Func<TRuleType, bool> predicate, string description);
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);
        TReturnType HaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType OnlyHaveAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);
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
        TReturnType DoNotCallAny(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DoNotCallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType DoNotCallAny(IEnumerable<MethodMember> methods);
        TReturnType DoNotCallAny(IObjectProvider<MethodMember> methods);
        TReturnType DoNotDependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotDependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DoNotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DoNotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DoNotDependOnAny(IObjectProvider<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<Type> types);
        TReturnType DoNotHaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DoNotHaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType DoNotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Type> attributes);
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