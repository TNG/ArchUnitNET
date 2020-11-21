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
    public class MethodCallDependency : IMemberMemberDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public MethodCallDependency(IMember originMember, MethodMember calledMethod,
            IEnumerable<GenericArgument> declaringTypeGenericArguments,
            IEnumerable<GenericArgument> calledMethodGenericArguments)
        {
            OriginMember = originMember;
            TargetMember = calledMethod;
            TargetGenericArguments = declaringTypeGenericArguments;
            TargetMemberGenericArguments = calledMethodGenericArguments;
        }

        public MethodCallDependency(IMember originMember, MethodMemberInstance calledMethodInstance)
            : this(originMember, calledMethodInstance.Member, calledMethodInstance.DeclaringTypeGenericArguments,
                calledMethodInstance.MemberGenericArguments)
        {
        }

        public IMember TargetMember { get; }
        public IMember OriginMember { get; }

        public IType Origin => OriginMember.DeclaringType;
        public IType Target => TargetMember.DeclaringType;

        public IEnumerable<GenericArgument> TargetGenericArguments { get; }
        public IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }

        public bool Equals(MethodCallDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(TargetMember, other.TargetMember) && Equals(OriginMember, other.OriginMember) &&
                   TargetGenericArguments.SequenceEqual(other.TargetGenericArguments) &&
                   TargetMemberGenericArguments.SequenceEqual(other.TargetMemberGenericArguments);
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

            return Equals((MethodCallDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TargetMember != null ? TargetMember.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (OriginMember != null ? OriginMember.GetHashCode() : 0);
                hashCode = TargetGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                hashCode = TargetMemberGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}