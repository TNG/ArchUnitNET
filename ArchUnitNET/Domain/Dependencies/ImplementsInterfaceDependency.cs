//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain.Dependencies
{
    public class ImplementsInterfaceDependency : ITypeDependency
    {
        public ImplementsInterfaceDependency(IType origin, IType target)
        {
            Origin = origin;
            Target = target;
        }

        public IType Origin { get; }
        public IType Target { get; }

        public bool Equals(ImplementsInterfaceDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Target, other.Target) && Equals(Origin, other.Origin);
        }

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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ImplementsInterfaceDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target != null ? Target.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Origin != null ? Origin.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}