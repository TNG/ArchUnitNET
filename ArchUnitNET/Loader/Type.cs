//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
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
        public Type(
            string fullname,
            string name,
            Assembly assembly,
            Namespace namespc,
            Visibility visibility,
            bool isNested,
            bool isGeneric,
            bool isStub,
            bool isCompilerGenerated
        )
        {
            FullName = fullname;
            Name = name;
            AssemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                assembly?.FullName ?? string.Empty,
                fullname
            );
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

        public string AssemblyQualifiedName { get; }

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
        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();

        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();

        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();

        public IEnumerable<IType> ImplementedInterfaces =>
            Dependencies
                .OfType<ImplementsInterfaceDependency>()
                .Select(dependency => dependency.Target);

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Type other)
        {
            return string.Equals(AssemblyQualifiedName, other.AssemblyQualifiedName);
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

            return obj.GetType() == GetType() && Equals((Type)obj);
        }

        public override int GetHashCode()
        {
            return AssemblyQualifiedName != null ? AssemblyQualifiedName.GetHashCode() : 0;
        }
    }
}
