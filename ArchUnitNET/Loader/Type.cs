﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Loader
{
    public class Type : IType
    {
        public Type(string fullname, string name, Assembly assembly, Namespace namespc, Visibility visibility,
            bool isNested, bool isGeneric, bool isStub, bool isCompilerGenerated)
        {
            FullName = fullname;
            Name = name;
            Assembly = assembly;
            Namespace = namespc;
            Visibility = visibility;
            IsNested = isNested;
            IsGeneric = isGeneric;
            IsStub = isStub;
            IsCompilerGenerated = isCompilerGenerated;
        }

        public bool IsAnonymousType { get; }

        public string Name { get; }

        public string FullName { get; }

        public Namespace Namespace { get; }

        public Assembly Assembly { get; }

        public Visibility Visibility { get; }

        public bool IsNested { get; }

        public bool IsGeneric { get; }
        public bool IsGenericParameter => false;
        public bool IsCompilerGenerated { get; }
        public List<GenericParameter> GenericParameters { get; } = new List<GenericParameter>();

        public bool IsStub { get; }
        public MemberList Members { get; } = new MemberList();
        public List<Attribute> Attributes { get; } = new List<Attribute>();

        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();

        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();

        public IEnumerable<IType> ImplementedInterfaces => Dependencies
            .OfType<ImplementsInterfaceDependency>()
            .Select(dependency => dependency.Target);

        public bool ImplementsInterface(Interface intf)
        {
            return ImplementedInterfaces.Any(implementedInterface =>
                Equals(implementedInterface, intf));
        }

        public bool ImplementsInterface(string pattern, bool useRegularExpressions = false)
        {
            if (pattern == null)
            {
                return false;
            }

            return ImplementedInterfaces.Any(implementedInterface =>
                implementedInterface.FullNameMatches(pattern, useRegularExpressions));
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

            return assignableToType is Interface && ImplementsInterface((Interface) assignableToType);
        }

        public bool IsAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            if (pattern == null)
            {
                return false;
            }

            return this.FullNameMatches(pattern, useRegularExpressions) ||
                   ImplementsInterface(pattern, useRegularExpressions);
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