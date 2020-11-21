//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.Dependencies
{
    public class GenericArgumentTypeDependency : ITypeDependency
    {
        public GenericArgumentTypeDependency(IType origin, IType target,
            IEnumerable<GenericArgument> targetGenericArguments)
        {
            Origin = origin;
            Target = target;
            TargetGenericArguments = targetGenericArguments;
        }

        public GenericArgumentTypeDependency(IType origin, GenericArgument targetInstance)
            : this(origin, targetInstance.Type, targetInstance.GenericArguments)
        {
        }

        public IType Origin { get; }
        public IType Target { get; }
        public IEnumerable<GenericArgument> TargetGenericArguments { get; }

        public bool Equals(GenericArgumentTypeDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Origin, other.Origin) && Equals(Target, other.Target) &&
                   TargetGenericArguments.SequenceEqual(other.TargetGenericArguments);
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

            return Equals((GenericArgumentTypeDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Origin != null ? Origin.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = TargetGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}