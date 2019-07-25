/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies.Members;

namespace ArchUnitNET.Domain
{
    public interface IMember : IHasName, IHasDependencies, IHasAttributes
    {
        IType DeclaringType { get; }
        List<IMemberTypeDependency> MemberDependencies { get; }
        List<IMemberTypeDependency> MemberBackwardsDependencies { get; }
    }
}