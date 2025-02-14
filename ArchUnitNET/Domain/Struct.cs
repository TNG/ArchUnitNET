//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class Struct : IType
    {
        public Struct(IType type)
        {
            Type = type;
        }

        public IType Type { get; }
        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string AssemblyQualifiedName => Type.AssemblyQualifiedName;

        [CanBeNull]
        public Class BaseClass =>
            (Class)Dependencies.OfType<InheritsBaseClassDependency>().FirstOrDefault()?.Target;

        public IEnumerable<Class> InheritedClasses =>
            BaseClass == null
                ? Enumerable.Empty<Class>()
                : BaseClass.InheritedClasses.Concat(new[] { BaseClass });

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;
        public bool IsGeneric => Type.IsGeneric;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsStub => Type.IsStub;
        public bool IsCompilerGenerated => Type.IsCompilerGenerated;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances => Type.AttributeInstances;

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;
        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;

        public MemberList Members => Type.Members;
        public List<GenericParameter> GenericParameters => Type.GenericParameters;

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Struct other)
        {
            return Equals(Type, other.Type);
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

            return obj.GetType() == GetType() && Equals((Struct)obj);
        }

        public override int GetHashCode()
        {
            return Type != null ? Type.GetHashCode() : 0;
        }
    }
}
