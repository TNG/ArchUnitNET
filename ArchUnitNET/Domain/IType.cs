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
        Namespace Namespace { get; }
        Assembly Assembly { get; }
        MemberList Members { get; }
        List<IType> GenericTypeParameters { get; }
        IEnumerable<IType> ImplementedInterfaces { get; }
        bool IsNested { get; }
        bool IsGeneric { get; }
        bool IsStub { get; }
        bool ImplementsInterface(Interface intf);
        bool ImplementsInterface(string pattern, bool useRegularExpressions = false);
        bool IsAssignableTo(IType assignableToType);
        bool IsAssignableTo(string pattern, bool useRegularExpressions = false);
    }
}