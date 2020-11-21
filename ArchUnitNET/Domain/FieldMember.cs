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
            return FullName.Equals(other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}