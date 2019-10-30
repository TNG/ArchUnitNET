/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;
using Equ;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Domain
{
    public class PropertyMember : MemberwiseEquatable<PropertyMember>, IMember
    {
        public PropertyMember(IType declaringType, string name, string fullName, IType type,
            bool isVirtual, [CanBeNull] MethodMember getter, [CanBeNull] MethodMember setter)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            DeclaringType = declaringType;
            IsVirtual = isVirtual;
            Getter = getter;
            Setter = setter;
        }

        public IType Type { get; }
        public bool IsVirtual { get; }
        public Visibility SetterVisibility => Setter?.Visibility ?? NotAccessible;
        public Visibility GetterVisibility => Getter?.Visibility ?? NotAccessible;

        [CanBeNull] public MethodMember Getter { get; }
        [CanBeNull] public MethodMember Setter { get; }
        public FieldMember BackingField { get; internal set; }

        public Visibility Visibility => GetterVisibility < SetterVisibility ? GetterVisibility : SetterVisibility;
        public string Name { get; }
        public string FullName { get; }
        public IType DeclaringType { get; }
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

            return obj.GetType() == GetType() && Equals((PropertyMember) obj);
        }

        private new bool Equals(PropertyMember other)
        {
            return base.Equals(other) && Equals(Type, other.Type) && IsVirtual == other.IsVirtual
                   && Equals(Getter, other.Getter) && Equals(Setter, other.Setter)
                   && Equals(BackingField, other.BackingField) && string.Equals(Name, other.Name)
                   && string.Equals(FullName, other.FullName) && Equals(DeclaringType, other.DeclaringType);
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