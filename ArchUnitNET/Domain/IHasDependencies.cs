/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Domain
{
    public interface IHasDependencies
    {
        List<ITypeDependency> Dependencies { get; }
        List<ITypeDependency> BackwardsDependencies { get; }
    }
}