/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

namespace ArchUnitNET.Domain.Dependencies.Types
{
    public interface ITypeDependency
    {
        IType Origin { get; }
        IType Target { get; }
    }
}