/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IType : IHasName, IHasDependencies, IImplementInterfaces, IHasAttributes
    {
        Namespace Namespace { get; }
        Assembly Assembly { get; }
        MemberList Members { get; }
        List<IType> GenericTypeParameters { get; }
        List<IType> GenericTypeArguments { get; }
        IType GenericType { get; }
        bool Implements(IType intf);
        bool IsAssignableTo(IType assignableToType);
    }
}