//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Domain
{
    public class PropertyMember : IMember
    {
        public PropertyMember(IType declaringType, string name, string fullName, IType type)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            DeclaringType = declaringType;
        }

        public IType Type { get; }
        public bool IsVirtual { get; internal set; }
        public bool IsAutoProperty { get; internal set; } = true;
        public Visibility SetterVisibility => Setter?.Visibility ?? NotAccessible;
        public Visibility GetterVisibility => Getter?.Visibility ?? NotAccessible;

        [CanBeNull] public MethodMember Getter { get; internal set; }

        [CanBeNull] public MethodMember Setter { get; internal set; }

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
                return setterDependencies.Concat(getterDependencies).Append(new PropertyTypeDependency(this)).ToList();
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
            return Equals(Type, other.Type) && IsVirtual == other.IsVirtual
                                            && IsAutoProperty == other.IsAutoProperty
                                            && Equals(Getter, other.Getter) && Equals(Setter, other.Setter) &&
                                            string.Equals(Name, other.Name)
                                            && string.Equals(FullName, other.FullName) &&
                                            Equals(DeclaringType, other.DeclaringType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsVirtual.GetHashCode();
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Getter != null ? Getter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Setter != null ? Setter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}