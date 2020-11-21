//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain.Dependencies
{
    public class TypeGenericParameterTypeConstraintDependency : ITypeDependency
    {
        public TypeGenericParameterTypeConstraintDependency(GenericParameter originGenericParameter,
            IType typeConstraint, IEnumerable<GenericArgument> typeConstraintGenericArguments)
        {
            OriginGenericParameter = originGenericParameter;
            Target = typeConstraint;
            TargetGenericArguments = typeConstraintGenericArguments;
        }

        public TypeGenericParameterTypeConstraintDependency(GenericParameter originGenericParameter,
            TypeInstance<IType> typeConstraintInstance)
            : this(originGenericParameter, typeConstraintInstance.Type, typeConstraintInstance.GenericArguments)
        {
        }

        public GenericParameter OriginGenericParameter { get; }

        public IType Origin => OriginGenericParameter.DeclaringType;
        public IType Target { get; }

        public IEnumerable<GenericArgument> TargetGenericArguments { get; }

        public bool Equals(TypeGenericParameterTypeConstraintDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Target, other.Target) && Equals(OriginGenericParameter, other.OriginGenericParameter) &&
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

            return Equals((TypeGenericParameterTypeConstraintDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target != null ? Target.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^
                           (OriginGenericParameter != null ? OriginGenericParameter.GetHashCode() : 0);
                hashCode = TargetGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}