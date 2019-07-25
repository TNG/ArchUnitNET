﻿/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class Class : IType
    {
        public Class(IType type, bool isAbstract)
        {
            Type = type;
            IsAbstract = isAbstract;
        }

        public IType Type { get; }

        public IEnumerable<ITypeDependency> DependenciesIncludingInherited => BaseClass != null
            ? Type.Dependencies.Concat(BaseClass.DependenciesIncludingInherited)
            : Type.Dependencies;

        public MemberList MembersIncludingInherited =>
            BaseClass != null
                ? new MemberList(Type.Members.Concat(BaseClass.MembersIncludingInherited).ToList())
                : Type.Members;

        public Class BaseClass =>
            (Class) Dependencies.OfType<InheritsBaseClassDependency>().FirstOrDefault()?.Target;

        public IEnumerable<MethodMember> Constructors => Type.GetConstructors();
        public bool IsAbstract { get; }
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

        public bool Implements(IType intf)
        {
            return Type.Implements(intf);
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
                    return Implements(@interface);
                case Class cls:
                    return BaseClass != null && BaseClass.IsAssignableTo(cls);
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Class other)
        {
            return Equals(Type, other.Type) && IsAbstract == other.IsAbstract;
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
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ IsAbstract.GetHashCode();
            }
        }
    }
}