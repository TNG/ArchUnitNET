//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassPredicates<out TReturnType, TRuleType>
        : ITypePredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();
        TReturnType AreImmutable();

        //Negations


        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
        TReturnType AreNotImmutable();
    }
}
