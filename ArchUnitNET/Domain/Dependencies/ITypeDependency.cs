//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public interface ITypeDependency
    {
        IType Origin { get; }
        IType Target { get; }

        IEnumerable<GenericArgument> TargetGenericArguments { get; }
        bool TargetIsArray { get; }
        IEnumerable<int> TargetArrayDimensions { get; }
    }
}