//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Domain.Dependencies
{
    public class AccessFieldDependency : IMemberMemberDependency
    {
        public AccessFieldDependency(IMember originMember, FieldMember accessedField)
        {
            OriginMember = originMember;
            TargetMember = accessedField;
        }

        public IMember OriginMember { get; }
        public IMember TargetMember { get; }

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

            return obj.GetType() == GetType() && Equals((AccessFieldDependency) obj);
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