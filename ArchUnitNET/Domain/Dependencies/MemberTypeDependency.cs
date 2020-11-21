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
    public class MemberTypeDependency : IMemberTypeDependency
    {
        public MemberTypeDependency(IMember originMember, IType target,
            IEnumerable<GenericArgument> targetGenericArguments)
        {
            Target = target;
            OriginMember = originMember;
            TargetGenericArguments = targetGenericArguments;
        }

        public MemberTypeDependency(IMember originMember, TypeInstance<IType> targetInstance)
            : this(originMember, targetInstance.Type, targetInstance.GenericArguments)
        {
        }

        public IType Origin => OriginMember.DeclaringType;
        public IMember OriginMember { get; }
        public IType Target { get; }
        public IEnumerable<GenericArgument> TargetGenericArguments { get; }

        public bool Equals(MemberTypeDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(OriginMember, other.OriginMember) && Equals(Target, other.Target) &&
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

            return Equals((MemberTypeDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginMember != null ? OriginMember.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = TargetGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}