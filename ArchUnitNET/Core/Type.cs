﻿/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Core
{
    public class Type : IType
    {
        public Type(string fullname, string name, Assembly assembly, Namespace namespc)
        {
            FullName = fullname;
            Name = name;
            Assembly = assembly;
            Namespace = namespc;
        }

        public string Name { get; }

        public string FullName { get; }

        public Namespace Namespace { get; }

        public Assembly Assembly { get; }

        public MemberList Members { get; } = new MemberList();
        public List<IType> GenericTypeParameters { get; set; }
        public IType GenericType { get; set; }

        public List<IType> GenericTypeArguments { get; set; }

        public List<Attribute> Attributes { get; } = new List<Attribute>();

        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();

        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();

        public IEnumerable<IType> ImplementedInterfaces => Dependencies
            .OfType<ImplementsInterfaceDependency>()
            .Select(dependency => dependency.Target);

        public bool Implements(IType @interface)
        {
            return ImplementedInterfaces.Any(implementedInterface =>
                Equals(implementedInterface, @interface) || Equals(implementedInterface.GenericType, @interface));
        }

        public bool IsAssignableTo(IType assignableToType)
        {
            if (assignableToType == null)
            {
                return false;
            }

            if (Equals(assignableToType))
            {
                return true;
            }

            return assignableToType is Interface && Implements(assignableToType);
        }

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Type other)
        {
            return string.Equals(FullName, other.FullName);
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

            return obj.GetType() == GetType() && Equals((Type) obj);
        }

        public override int GetHashCode()
        {
            return FullName != null ? FullName.GetHashCode() : 0;
        }
    }
}