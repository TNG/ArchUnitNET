//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        TReturnType BeConstructor();
        TReturnType BeVirtual();
        TReturnType BeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType BeCalledBy(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType BeCalledBy(IType firstType, params IType[] moreTypes);
        TReturnType BeCalledBy(Type type, params Type[] moreTypes);
        TReturnType BeCalledBy(IObjectProvider<IType> types);
        TReturnType BeCalledBy(IEnumerable<IType> types);
        TReturnType BeCalledBy(IEnumerable<Type> types);
        TReturnType HaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
        TReturnType HaveDependencyInMethodBodyTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TReturnType HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TReturnType HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TReturnType HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TReturnType HaveDependencyInMethodBodyTo(IEnumerable<Type> types);


        //Negations


        TReturnType BeNoConstructor();
        TReturnType NotBeVirtual();
        TReturnType NotBeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType NotBeCalledBy(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBeCalledBy(IType firstType, params IType[] moreTypes);
        TReturnType NotBeCalledBy(Type type, params Type[] moreTypes);
        TReturnType NotBeCalledBy(IObjectProvider<IType> types);
        TReturnType NotBeCalledBy(IEnumerable<IType> types);
        TReturnType NotBeCalledBy(IEnumerable<Type> types);
        TReturnType NotHaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveDependencyInMethodBodyTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TReturnType NotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TReturnType NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TReturnType NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TReturnType NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);
    }
}