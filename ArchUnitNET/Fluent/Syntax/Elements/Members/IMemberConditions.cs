//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberConditions<out TReturnType, out TRuleType>
        : IObjectConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TReturnType BeDeclaredIn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TReturnType BeDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType BeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType BeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType BeDeclaredIn(IObjectProvider<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<Type> types);
        TReturnType BeStatic();
        TReturnType BeImmutable();

        //Negations
        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBeDeclaredIn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBeDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotBeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType NotBeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType NotBeDeclaredIn(IObjectProvider<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<Type> types);
        TReturnType NotBeStatic();
        TReturnType NotBeImmutable();
    }
}
