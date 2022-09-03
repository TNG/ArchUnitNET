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
    public interface
        IMemberPredicates<out TRuleTypeConjunction, TRuleType> : IObjectPredicates<TRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TRuleTypeConjunction AreDeclaredIn(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreStatic();
        TRuleTypeConjunction AreImmutable();

        //Negations


        TRuleTypeConjunction AreNotDeclaredIn(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreNotStatic();
        TRuleTypeConjunction AreNotImmutable();
    }
}