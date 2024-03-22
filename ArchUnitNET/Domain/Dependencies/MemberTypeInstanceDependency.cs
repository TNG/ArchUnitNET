//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

namespace ArchUnitNET.Domain.Dependencies
{
    public abstract class MemberTypeInstanceDependency
        : TypeInstanceDependency,
            IMemberTypeDependency
    {
        protected MemberTypeInstanceDependency(
            IMember originMember,
            ITypeInstance<IType> targetInstance
        )
            : base(originMember.DeclaringType, targetInstance)
        {
            OriginMember = originMember;
        }

        public IMember OriginMember { get; }

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

            return obj.GetType() == GetType() && Equals((MemberTypeInstanceDependency)obj);
        }

        private bool Equals(MemberTypeInstanceDependency other)
        {
            return Equals(OriginMember, other.OriginMember)
                && Equals(TargetInstance, other.TargetInstance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginMember != null ? OriginMember.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397) ^ (TargetInstance != null ? TargetInstance.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
