/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IImplementInterfaces
    {
        IEnumerable<IType> ImplementedInterfaces { get; }
    }
}