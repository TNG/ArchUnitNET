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
    public interface IMethodMemberPredicates<out TRuleTypeConjunction, TRuleType>
        : IMemberPredicates<TRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TRuleTypeConjunction AreConstructors();
        TRuleTypeConjunction AreVirtual();
        TRuleTypeConjunction AreCalledBy(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreCalledBy(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreCalledBy(IObjectProvider<IType> types);
        TRuleTypeConjunction AreCalledBy(IEnumerable<IType> types);
        TRuleTypeConjunction AreCalledBy(IEnumerable<Type> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        );
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types);
        TRuleTypeConjunction HaveReturnType(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction HaveReturnType(IEnumerable<IType> types);
        TRuleTypeConjunction HaveReturnType(IObjectProvider<IType> types);
        TRuleTypeConjunction HaveReturnType(Type type, params Type[] moreTypes);
        TRuleTypeConjunction HaveReturnType(IEnumerable<Type> types);

        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
        TRuleTypeConjunction AreNotCalledBy(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotCalledBy(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreNotCalledBy(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotCalledBy(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotCalledBy(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        );
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveReturnType(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DoNotHaveReturnType(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotHaveReturnType(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotHaveReturnType(Type type, params Type[] moreTypes);
        TRuleTypeConjunction DoNotHaveReturnType(IEnumerable<Type> types);
    }
}
