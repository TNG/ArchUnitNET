/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies.Members;

namespace ArchUnitNET.Domain
{
    public interface IMember : ICanBeAnalyzed
    {
        IType DeclaringType { get; }
        List<IMemberTypeDependency> MemberDependencies { get; }
        List<IMemberTypeDependency> MemberBackwardsDependencies { get; }
    }
}