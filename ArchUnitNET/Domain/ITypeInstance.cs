//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface ITypeInstance<out T>
        where T : IType
    {
        T Type { get; }
        IEnumerable<GenericArgument> GenericArguments { get; }
        bool IsArray { get; }
        IEnumerable<int> ArrayDimensions { get; }
    }
}
