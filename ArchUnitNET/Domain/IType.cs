//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IType : ICanBeAnalyzed
    {
        MemberList Members { get; }
        IEnumerable<IType> ImplementedInterfaces { get; }
        bool IsNested { get; }
        bool IsStub { get; }
        bool IsGenericParameter { get; }
    }
}
