//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class FieldMember : IMember
    {
        public FieldMember(IType declaringType, string name, string fullName, Visibility visibility, IType type)
        {
            DeclaringType = declaringType;
            Name = name;
            FullName = fullName;
            Visibility = visibility;
            Type = type;
        }

        public IType Type { get; }
        public Visibility Visibility { get; }

        public IType DeclaringType { get; }
        public string Name { get; }
        public string FullName { get; }
        public List<Attribute> Attributes { get; } = new List<Attribute>();
        public List<IMemberTypeDependency> MemberDependencies { get; } = new List<IMemberTypeDependency>();
        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } = new List<IMemberTypeDependency>();
        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();
        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();

        public override string ToString()
        {
            return $"{DeclaringType.FullName}{'.'}{Name}";
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

            return obj.GetType() == GetType() && Equals((FieldMember) obj);
        }

        private bool Equals(FieldMember other)
        {
            return Equals(DeclaringType, other.DeclaringType) && string.Equals(Name, other.Name)
                                                              && string.Equals(FullName, other.FullName)
                                                              && Equals(Type, other.Type)
                                                              && Visibility == other.Visibility;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeclaringType != null ? DeclaringType.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Visibility;
                return hashCode;
            }
        }
    }
}