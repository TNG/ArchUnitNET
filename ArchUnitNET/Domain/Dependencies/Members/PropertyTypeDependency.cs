/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using Equ;

namespace ArchUnitNET.Domain.Dependencies.Members
{
    public class PropertyTypeDependency : MemberwiseEquatable<PropertyTypeDependency>, IMemberTypeDependency
    {
        private readonly PropertyMember _originMember;

        public PropertyTypeDependency(PropertyMember property)
        {
            _originMember = property;
        }

        public IType Target => _originMember.Type;
        public IMember OriginMember => _originMember;

        public IType Origin => _originMember.DeclaringType;

        public new bool Equals(PropertyTypeDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(_originMember, other._originMember);
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

            return Equals((PropertyTypeDependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (_originMember != null ? _originMember.GetHashCode() : 0);
            }
        }
    }
}