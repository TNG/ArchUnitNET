//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        TReturnType BeConstructor();
        TReturnType BeVirtual();
        TReturnType BeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType HaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);


        //Negations


        TReturnType BeNoConstructor();
        TReturnType NotBeVirtual();
        TReturnType NotBeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
    }
}