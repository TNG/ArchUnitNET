//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
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
    public interface ITypeConditions<out TReturnType, out TRuleType>
        : IObjectConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType Be(Type firstType, params Type[] moreTypes);
        TReturnType Be(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType BeAssignableTo(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType BeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType BeAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType BeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType BeAssignableTo(IObjectProvider<IType> types);
        TReturnType BeAssignableTo(IEnumerable<IType> types);
        TReturnType BeAssignableTo(IEnumerable<Type> types);
        TReturnType BeValueTypes();
        TReturnType BeEnums();
        TReturnType BeStructs();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType ImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType ImplementInterface(Interface intf);
        TReturnType ImplementInterface(Type intf);

        [Obsolete(
            "Either ResideInNamespace() without the useRegularExpressions parameter or ResideInNamespaceMatching() should be used"
        )]
        TReturnType ResideInNamespace(string pattern, bool useRegularExpressions);
        TReturnType ResideInNamespace(string fullName);
        TReturnType ResideInNamespaceMatching(string pattern);

        [Obsolete(
            "Either ResideInAssembly() without the useRegularExpressions parameter or ResideInAssemblyMatching() should be used"
        )]
        TReturnType ResideInAssembly(string pattern, bool useRegularExpressions);
        TReturnType ResideInAssembly(string fullName);
        TReturnType ResideInAssemblyMatching(string pattern);
        TReturnType ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType BeNested();

        //Negations


        TReturnType NotBe(Type firstType, params Type[] moreTypes);
        TReturnType NotBe(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBeAssignableTo(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotBeAssignableTo(IType type, params IType[] moreTypes);
        TReturnType NotBeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType NotBeAssignableTo(IObjectProvider<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<Type> types);
        TReturnType NotBeValueTypes();
        TReturnType NotBeEnums();
        TReturnType NotBeStructs();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType NotImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType NotImplementInterface(Interface intf);
        TReturnType NotImplementInterface(Type intf);

        [Obsolete(
            "Either NotResideInNamespace() without the useRegularExpressions parameter or NotResideInNamespaceMatching() should be used"
        )]
        TReturnType NotResideInNamespace(string pattern, bool useRegularExpressions);
        TReturnType NotResideInNamespace(string fullName);
        TReturnType NotResideInNamespaceMatching(string pattern);

        [Obsolete(
            "Either NotResideInAssembly() without the useRegularExpressions parameter or NotResideInAssemblyMatching() should be used"
        )]
        TReturnType NotResideInAssembly(string fullName);
        TReturnType NotResideInAssemblyMatching(string pattern);
        TReturnType NotResideInAssembly(string pattern, bool useRegularExpressions);
        TReturnType NotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType NotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType NotHavePropertyMemberWithName(string name);
        TReturnType NotHaveFieldMemberWithName(string name);
        TReturnType NotHaveMethodMemberWithName(string name);
        TReturnType NotHaveMemberWithName(string name);
        TReturnType NotBeNested();
    }
}
