/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using Equ;

namespace ArchUnitNET.Domain.Dependencies.Types
{
    public class TypeReferenceDependency : MemberwiseEquatable<TypeReferenceDependency>, ITypeDependency
    {
        public TypeReferenceDependency(IType origin, IType target)
        {
            Origin = origin;
            Target = target;
        }

        public IType Origin { get; }
        public IType Target { get; }
    }
}