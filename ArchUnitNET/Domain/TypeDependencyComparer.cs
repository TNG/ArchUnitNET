//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class TypeDependencyComparer : IEqualityComparer<ITypeDependency>
    {
        public bool Equals(ITypeDependency x, ITypeDependency y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return x != null
                && y != null
                && Equals(x.Origin, y.Origin)
                && Equals(x.Target, y.Target);
        }

        public int GetHashCode(ITypeDependency obj)
        {
            unchecked
            {
                return (obj.Origin.GetHashCode() * 397) ^ obj.Target.GetHashCode();
            }
        }
    }
}
