/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

namespace ArchUnitNET.Domain
{
    public interface IHasName
    {
        string Name { get; }
        string FullName { get; }
    }
}