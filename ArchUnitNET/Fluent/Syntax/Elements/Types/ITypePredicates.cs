﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<out TReturnType, TRuleType>
        : IObjectPredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType Are(Type firstType, params Type[] moreTypes);
        TReturnType Are(IEnumerable<Type> types);
        TReturnType AreAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType AreAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreAssignableTo(IObjectProvider<IType> types);
        TReturnType AreAssignableTo(IEnumerable<IType> types);
        TReturnType AreAssignableTo(IEnumerable<Type> types);
        TReturnType AreValueTypes();
        TReturnType AreEnums();
        TReturnType AreStructs();
        TReturnType ImplementInterface(Interface intf);
        TReturnType ImplementInterface(Type intf);
        TReturnType ResideInNamespace(string pattern, bool useRegularExpressions = false); //TODO split into multiple implementations
        TReturnType ResideInAssembly(string pattern, bool useRegularExpressions = false); //TODO split into multiple implementations
        TReturnType ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType AreNested();

        //Negations


        TReturnType AreNot(Type firstType, params Type[] moreTypes);
        TReturnType AreNot(IEnumerable<Type> types);
        TReturnType AreNotAssignableTo(IType type, params IType[] moreTypes);
        TReturnType AreNotAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreNotAssignableTo(IObjectProvider<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<Type> types);
        TReturnType AreNotValueTypes();
        TReturnType AreNotEnums();
        TReturnType AreNotStructs();
        TReturnType DoNotImplementInterface(Interface intf);
        TReturnType DoNotImplementInterface(Type intf);
        TReturnType DoNotResideInNamespace(string pattern, bool useRegularExpressions = false); //TODO split into multiple implementations
        TReturnType DoNotResideInAssembly(string pattern, bool useRegularExpressions = false); //TODO split into multiple implementations
        TReturnType DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType DoNotHavePropertyMemberWithName(string name);
        TReturnType DoNotHaveFieldMemberWithName(string name);
        TReturnType DoNotHaveMethodMemberWithName(string name);
        TReturnType DoNotHaveMemberWithName(string name);
        TReturnType AreNotNested();
    }
}
