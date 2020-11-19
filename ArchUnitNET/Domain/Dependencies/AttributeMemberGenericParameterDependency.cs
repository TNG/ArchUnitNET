//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Domain.Dependencies
{
    public class AttributeMemberGenericParameterDependency : IMemberTypeDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public AttributeMemberGenericParameterDependency(IMember member, GenericParameter originGenericParameter,
            Attribute attribute)
        {
            OriginMember = member;
            OriginGenericParameter = originGenericParameter;
            Target = attribute;
        }

        public GenericParameter OriginGenericParameter { get; }

        public IType Target { get; }

        public IMember OriginMember { get; }

        public IType Origin => OriginMember.DeclaringType;

        public bool Equals(AttributeMemberGenericParameterDependency other)
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

            return Equals((AttributeMemberGenericParameterDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target != null ? Target.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (OriginMember != null ? OriginMember.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^
                           (OriginGenericParameter != null ? OriginGenericParameter.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}