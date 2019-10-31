//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassPredicates<out TReturnType> : ITypePredicates<TReturnType>
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();
        TReturnType AreValueTypes();
        TReturnType AreEnums();
        TReturnType AreStructs();


        //Negations


        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
        TReturnType AreNotValueTypes();
        TReturnType AreNotEnums();
        TReturnType AreNotStructs();
    }
}