//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader
{
    public class MethodMemberInstance
    {
        public MethodMemberInstance(MethodMember member, IEnumerable<GenericArgument> declaringTypeGenericArguments,
            IEnumerable<GenericArgument> memberGenericArguments)
        {
            Member = member;
            DeclaringTypeGenericArguments = declaringTypeGenericArguments;
            MemberGenericArguments = memberGenericArguments;
        }

        public MethodMember Member { get; }
        public IEnumerable<GenericArgument> DeclaringTypeGenericArguments { get; }
        public IEnumerable<GenericArgument> MemberGenericArguments { get; }

        public bool Equals(MethodMemberInstance other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Member, other.Member) &&
                   DeclaringTypeGenericArguments.SequenceEqual(other.DeclaringTypeGenericArguments) &&
                   MemberGenericArguments.SequenceEqual(other.MemberGenericArguments);
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

            return Equals((MethodMemberInstance) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Member != null ? Member.GetHashCode() : 0;
                hashCode = DeclaringTypeGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                hashCode = MemberGenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}