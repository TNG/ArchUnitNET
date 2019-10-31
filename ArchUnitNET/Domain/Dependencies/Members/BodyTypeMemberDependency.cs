//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using Equ;

namespace ArchUnitNET.Domain.Dependencies.Members
{
    public class BodyTypeMemberDependency : MemberwiseEquatable<BodyTypeMemberDependency>,
        IMemberTypeDependency
    {
        public BodyTypeMemberDependency(MethodMember method, IType target)
        {
            OriginMember = method;
            Target = target;
        }

        public IMember OriginMember { get; }

        public IType Origin => OriginMember.DeclaringType;
        public IType Target { get; }

        public new bool Equals(BodyTypeMemberDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(OriginMember, other.OriginMember) && Equals(Target, other.Target);
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

            return Equals((BodyTypeMemberDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (OriginMember != null ? OriginMember.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}