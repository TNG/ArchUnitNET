//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodCallDependency : IMemberMemberDependency
    {
        public MethodCallDependency(IMember originMember, MethodMember calledMethod)
        {
            OriginMember = originMember;
            TargetMember = calledMethod;
        }

        public IMember TargetMember { get; }
        public IMember OriginMember { get; }

        public IType Origin => OriginMember.DeclaringType;
        public IType Target => TargetMember.DeclaringType;

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

            return obj.GetType() == GetType() && Equals((MethodCallDependency) obj);
        }

        private bool Equals(IMemberMemberDependency other)
        {
            return Equals(TargetMember, other.TargetMember) && Equals(OriginMember, other.OriginMember);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TargetMember != null ? TargetMember.GetHashCode() : 0) * 397) ^
                       (OriginMember != null ? OriginMember.GetHashCode() : 0);
            }
        }
    }
}