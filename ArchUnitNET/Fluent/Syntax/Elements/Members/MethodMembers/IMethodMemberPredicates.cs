//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberPredicates<out TRuleTypeConjunction> : IMemberPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreConstructors();
        TRuleTypeConjunction AreVirtual();
        TRuleTypeConjunction AreCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);


        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
        TRuleTypeConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
    }
}