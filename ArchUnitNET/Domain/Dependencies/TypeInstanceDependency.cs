//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public abstract class TypeInstanceDependency : ITypeDependency
    {
        protected readonly ITypeInstance<IType> TargetInstance;

        protected TypeInstanceDependency(IType origin, ITypeInstance<IType> targetInstance)
        {
            Origin = origin;
            TargetInstance = targetInstance;
        }

        public IType Origin { get; }
        public IType Target => TargetInstance.Type;
        public IEnumerable<GenericArgument> TargetGenericArguments => TargetInstance.GenericArguments;
        public bool TargetIsArray => TargetInstance.IsArray;
        public IEnumerable<int> TargetArrayDimensions => TargetInstance.ArrayDimensions;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((TypeInstanceDependency) obj);
        }

        private bool Equals(TypeInstanceDependency other)
        {
            return Equals(Origin, other.Origin) && Equals(TargetInstance, other.TargetInstance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Origin != null ? Origin.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (TargetInstance != null ? TargetInstance.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}