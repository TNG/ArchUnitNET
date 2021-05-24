﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IGenericTypeInstance : IType
    {
        IType ElementType { get; }
        IEnumerable<IType> GenericArguments { get; }
    }
}