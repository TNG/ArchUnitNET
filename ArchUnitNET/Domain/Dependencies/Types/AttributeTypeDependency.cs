//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using Equ;

namespace ArchUnitNET.Domain.Dependencies.Types
{
    public class AttributeTypeDependency : MemberwiseEquatable<AttributeTypeDependency>, ITypeDependency
    {
        public AttributeTypeDependency(IType origin, Attribute target)
        {
            Origin = origin;
            Target = target;
        }

        public IType Origin { get; }
        public IType Target { get; }
    }
}