﻿using System;
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
        TRuleTypeConjunction AreCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction AreCalledBy(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction AreCalledBy(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreCalledBy(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreCalledBy(IObjectProvider<IType> types);
        TRuleTypeConjunction AreCalledBy(IEnumerable<IType> types);
        TRuleTypeConjunction AreCalledBy(IEnumerable<Type> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        );

        TRuleTypeConjunction HaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );

        TRuleTypeConjunction HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        );
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types);
        TRuleTypeConjunction HaveReturnType(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction HaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction HaveReturnType(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction HaveReturnType(IEnumerable<IType> types);
        TRuleTypeConjunction HaveReturnType(IObjectProvider<IType> types);
        TRuleTypeConjunction HaveReturnType(Type type, params Type[] moreTypes);
        TRuleTypeConjunction HaveReturnType(IEnumerable<Type> types);

        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
        TRuleTypeConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction AreNotCalledBy(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction AreNotCalledBy(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotCalledBy(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreNotCalledBy(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotCalledBy(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotCalledBy(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        );

        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );

        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        );
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TRuleTypeConjunction DoNotHaveReturnType(
            string pattern,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction DoNotHaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction DoNotHaveReturnType(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction DoNotHaveReturnType(IEnumerable<IType> types);
        TRuleTypeConjunction DoNotHaveReturnType(IObjectProvider<IType> types);
        TRuleTypeConjunction DoNotHaveReturnType(Type type, params Type[] moreTypes);
        TRuleTypeConjunction DoNotHaveReturnType(IEnumerable<Type> types);
    }
}
