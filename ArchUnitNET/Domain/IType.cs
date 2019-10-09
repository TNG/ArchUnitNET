/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IType : ICanBeAnalyzed
    {
        Namespace Namespace { get; }
        Assembly Assembly { get; }
        MemberList Members { get; }
        List<IType> GenericTypeParameters { get; }
        List<IType> GenericTypeArguments { get; }
        IType GenericType { get; }
        bool IsNested { get; }
        IEnumerable<IType> ImplementedInterfaces { get; }
        bool Implements(IType intf);
        bool Implements(string interfacePattern);
        bool IsAssignableTo(IType assignableToType);
        bool IsAssignableTo(string pattern);
    }
}