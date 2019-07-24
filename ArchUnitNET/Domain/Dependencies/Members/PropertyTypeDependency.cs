/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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