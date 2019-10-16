/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class Class : IType
    {
        public Class(IType type, bool? isAbstract = null, bool? isSealed = null, bool? isValueType = null,
            bool? isEnum = null)
        {
            Type = type;
            IsAbstract = isAbstract;
            IsSealed = isSealed;
            IsValueType = isValueType;
            IsEnum = isEnum;
        }

        public IType Type { get; }

        public IEnumerable<ITypeDependency> DependenciesIncludingInherited => BaseClass != null
            ? Type.Dependencies.Concat(BaseClass.DependenciesIncludingInherited)
            : Type.Dependencies;

        public MemberList MembersIncludingInherited =>
            BaseClass != null
                ? new MemberList(Type.Members.Concat(BaseClass.MembersIncludingInherited).ToList())
                : Type.Members;

        [CanBeNull]
        public Class BaseClass =>
            (Class) Dependencies.OfType<InheritsBaseClassDependency>().FirstOrDefault()?.Target;

        public IEnumerable<MethodMember> Constructors => Type.GetConstructors();
        public bool? IsAbstract { get; }
        public bool? IsSealed { get; }
        public bool? IsValueType { get; }
        public bool? IsEnum { get; }

        public bool? IsStruct
        {
            get
            {
                if (IsValueType.HasValue && IsEnum.HasValue)
                {
                    return IsValueType.Value && !IsEnum.Value;
                }

                return null;
            }
        }

        public IEnumerable<Class> InheritedClasses => BaseClass == null
            ? Enumerable.Empty<Class>()
            : BaseClass.InheritedClasses.Append(BaseClass);

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;
        public string Name => Type.Name;
        public string FullName => Type.FullName;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;

        public List<Attribute> Attributes { get; } = new List<Attribute>();

        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;
        public MemberList Members => Type.Members;


        public List<IType> GenericTypeParameters => Type.GenericTypeParameters;
        public IType GenericType => Type.GenericType;
        public List<IType> GenericTypeArguments => Type.GenericTypeArguments;

        public bool ImplementsInterface(IType intf)
        {
            return Type.ImplementsInterface(intf);
        }

        public bool ImplementsInterface(string pattern, bool useRegularExpressions = false)
        {
            return Type.ImplementsInterface(pattern, useRegularExpressions);
        }

        public bool IsAssignableTo(IType assignableToType)
        {
            if (Equals(assignableToType, this))
            {
                return true;
            }

            switch (assignableToType)
            {
                case Interface @interface:
                    return ImplementsInterface(@interface);
                case Class cls:
                    return BaseClass != null && BaseClass.IsAssignableTo(cls);
                default:
                    return false;
            }
        }

        public bool IsAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            if (pattern == null)
            {
                return false;
            }

            if (this.FullNameMatches(pattern, useRegularExpressions) ||
                ImplementsInterface(pattern, useRegularExpressions))
            {
                return true;
            }

            return BaseClass != null && BaseClass.IsAssignableTo(pattern, useRegularExpressions);
        }

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Class other)
        {
            return Equals(Type, other.Type) && Equals(IsAbstract, other.IsAbstract) &&
                   Equals(IsSealed, other.IsSealed) && Equals(IsValueType, other.IsValueType) &&
                   Equals(IsEnum, other.IsEnum);
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

            return obj.GetType() == GetType() && Equals((Class) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ IsAbstract.GetHashCode();
                hashCode = (hashCode * 397) ^ IsSealed.GetHashCode();
                hashCode = (hashCode * 397) ^ IsValueType.GetHashCode();
                hashCode = (hashCode * 397) ^ IsEnum.GetHashCode();
                return hashCode;
            }
        }
    }
}