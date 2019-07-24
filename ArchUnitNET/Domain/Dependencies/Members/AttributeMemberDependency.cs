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
    public class AttributeMemberDependency : MemberwiseEquatable<AttributeMemberDependency>, IMemberTypeDependency
    {
        public AttributeMemberDependency(IMember member, Attribute attribute)
        {
            OriginMember = member;
            Target = attribute;
        }

        public IType Target { get; } //attribute

        public IMember OriginMember { get; } //object with attribute

        public IType Origin => OriginMember.DeclaringType; //class of object with attribute
        
        public new bool Equals(AttributeMemberDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(Target, other.Target) && Equals(OriginMember, other.OriginMember);
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
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OriginMember != null ? OriginMember.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}