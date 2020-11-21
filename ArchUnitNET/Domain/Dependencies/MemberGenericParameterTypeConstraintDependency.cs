//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MemberGenericParameterTypeConstraintDependency : IMemberTypeDependency
    {
        public MemberGenericParameterTypeConstraintDependency(GenericParameter originGenericParameter,
            IType typeConstraint, IEnumerable<GenericArgument> typeConstraintGenericArguments)
        {
            if (originGenericParameter.DeclaringMember == null)
            {
                throw new ArgumentException(
                    "Use TypeGenericParameterTypeConstraintDependency for Generic Parameters of Types.");
            }

            OriginGenericParameter = originGenericParameter;
            Target = typeConstraint;
            TargetGenericArguments = typeConstraintGenericArguments;
        }

        public MemberGenericParameterTypeConstraintDependency(GenericParameter originGenericParameter,
            TypeInstance<IType> typeConstraintInstance)
            : this(originGenericParameter, typeConstraintInstance.Type, typeConstraintInstance.GenericArguments)
        {
        }

        public GenericParameter OriginGenericParameter { get; }

        public IType Target { get; }

        public IEnumerable<GenericArgument> TargetGenericArguments { get; }

        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull] public IMember OriginMember => OriginGenericParameter.DeclaringMember;

        public IType Origin => OriginMember.DeclaringType;

        public bool Equals(MemberGenericParameterTypeConstraintDependency other)
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

            return Equals((MemberGenericParameterTypeConstraintDependency) obj);
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