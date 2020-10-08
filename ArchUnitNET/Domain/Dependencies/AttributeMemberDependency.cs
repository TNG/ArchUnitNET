//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain.Dependencies
{
    public class AttributeMemberDependency : IMemberTypeDependency
    {
        public AttributeMemberDependency(IMember member, Attribute attribute)
        {
            OriginMember = member;
            Target = attribute;
        }

        public IType Target { get; } //attribute

        public IMember OriginMember { get; } //object with attribute

        public IType Origin => OriginMember.DeclaringType; //class of object with attribute

        public bool Equals(AttributeMemberDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Target, other.Target) && Equals(OriginMember, other.OriginMember);
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

            return Equals((AttributeMemberDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target != null ? Target.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (OriginMember != null ? OriginMember.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}