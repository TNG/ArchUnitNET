//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain.Dependencies
{
    public class AttributeTypeDependency : ITypeDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public AttributeTypeDependency(IType origin, Attribute target,
            IEnumerable<GenericArgument> attributeGenericArguments)
        {
            Origin = origin;
            Target = target;
            TargetGenericArguments = attributeGenericArguments;
        }

        public AttributeTypeDependency(IType origin, TypeInstance<Attribute> attributeInstance)
            : this(origin, attributeInstance.Type, attributeInstance.GenericArguments)
        {
        }

        public IType Origin { get; }
        public IType Target { get; }
        public IEnumerable<GenericArgument> TargetGenericArguments { get; }

        public bool Equals(AttributeTypeDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Target, other.Target) && Equals(Origin, other.Origin) &&
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

            return Equals((AttributeTypeDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target != null ? Target.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Origin != null ? Origin.GetHashCode() : 0);
                hashCode = TargetGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}