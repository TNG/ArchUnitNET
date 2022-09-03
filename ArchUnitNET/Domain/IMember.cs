//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public interface IMember : ICanBeAnalyzed
    {
        IType DeclaringType { get; }
        List<IMemberTypeDependency> MemberDependencies { get; }
        List<IMemberTypeDependency> MemberBackwardsDependencies { get; }
        bool? IsStatic { get; }
        bool? IsReadOnly { get; }
        bool? IsImmutable { get; }
    }
}