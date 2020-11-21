//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Loader;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Domain
{
    public class PropertyMember : IMember
    {
        private readonly TypeInstance<IType> _typeInstance;

        public PropertyMember(IType declaringType, string name, string fullName, TypeInstance<IType> type)
        {
            Name = name;
            FullName = fullName;
            _typeInstance = type;
            DeclaringType = declaringType;
            PropertyTypeDependency = new PropertyTypeDependency(this);
        }

        public IType Type => _typeInstance.Type;
        public IEnumerable<GenericArgument> TypeGenericArguments => _typeInstance.GenericArguments;
        public bool IsVirtual { get; internal set; }
        public bool IsAutoProperty { get; internal set; } = true;
        public Visibility SetterVisibility => Setter?.Visibility ?? NotAccessible;
        public Visibility GetterVisibility => Getter?.Visibility ?? NotAccessible;

        [CanBeNull] public MethodMember Getter { get; internal set; }

        [CanBeNull] public MethodMember Setter { get; internal set; }

        public List<IMemberTypeDependency> AttributeDependencies { get; } = new List<IMemberTypeDependency>();

        public IMemberTypeDependency PropertyTypeDependency { get; }

        public bool IsGeneric => false;
        public List<GenericParameter> GenericParameters => new List<GenericParameter>();

        public Visibility Visibility => GetterVisibility < SetterVisibility ? GetterVisibility : SetterVisibility;
        public string Name { get; }
        public string FullName { get; }
        public IType DeclaringType { get; }
        public List<Attribute> Attributes { get; } = new List<Attribute>();

        public List<IMemberTypeDependency> MemberDependencies
        {
            get
            {
                var setterDependencies = Setter?.MemberDependencies ?? Enumerable.Empty<IMemberTypeDependency>();
                var getterDependencies = Getter?.MemberDependencies ?? Enumerable.Empty<IMemberTypeDependency>();
                return setterDependencies.Concat(getterDependencies).Concat(AttributeDependencies)
                    .Append(PropertyTypeDependency).ToList();
            }
        }

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

            return obj.GetType() == GetType() && Equals((PropertyMember) obj);
        }

        private bool Equals(PropertyMember other)
        {
            return Equals(FullName, other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}