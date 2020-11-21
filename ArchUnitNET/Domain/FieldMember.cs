//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain
{
    public class FieldMember : IMember
    {
        private readonly TypeInstance<IType> _typeInstance;

        public FieldMember(IType declaringType, string name, string fullName, Visibility visibility,
            TypeInstance<IType> typeInstance)
        {
            DeclaringType = declaringType;
            Name = name;
            FullName = fullName;
            Visibility = visibility;
            _typeInstance = typeInstance;
        }

        public IType Type => _typeInstance.Type;
        public IEnumerable<GenericArgument> GenericArguments => _typeInstance.GenericArguments;
        public Visibility Visibility { get; }

        public IType DeclaringType { get; }
        public string Name { get; }
        public string FullName { get; }

        public bool IsGeneric => false;
        public List<GenericParameter> GenericParameters => new List<GenericParameter>();
        public List<Attribute> Attributes { get; } = new List<Attribute>();
        public List<IMemberTypeDependency> MemberDependencies { get; } = new List<IMemberTypeDependency>();
        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } = new List<IMemberTypeDependency>();
        public List<ITypeDependency> Dependencies => MemberDependencies.Cast<ITypeDependency>().ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            MemberBackwardsDependencies.Cast<ITypeDependency>().ToList();

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
                                                              && Visibility == other.Visibility
                                                              && GenericArguments.SequenceEqual(other.GenericArguments);
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
                hashCode = GenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}