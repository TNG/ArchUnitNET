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
    public class Class : IType
    {
        private IType Type { get; }

        public Class(IType type, bool? isAbstract = null, bool? isSealed = null)
        {
            Type = type;
            IsAbstract = isAbstract;
            IsSealed = isSealed;
        }

        public Class(Class @class)
        {
            Type = @class.Type;
            IsAbstract = @class.IsAbstract;
            IsSealed = @class.IsSealed;
        }

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

        public IEnumerable<Class> InheritedClasses => BaseClass == null
            ? Enumerable.Empty<Class>()
            : BaseClass.InheritedClasses.Concat(new[] {BaseClass});

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;
        public bool IsGeneric => Type.IsGeneric;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsStub => Type.IsStub;
        public bool IsCompilerGenerated => Type.IsCompilerGenerated;
        public string Name => Type.Name;
        public string FullName => Type.FullName;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;

        public IEnumerable<Attribute> Attributes => AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances => Type.AttributeInstances;

        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;
        public MemberList Members => Type.Members;

        public List<GenericParameter> GenericParameters => Type.GenericParameters;

        public bool ImplementsInterface(Interface intf)
        {
            return Type.ImplementsInterface(intf);
        }

        public bool ImplementsInterface(string pattern, bool useRegularExpressions = false)
        {
            return Type.ImplementsInterface(pattern, useRegularExpressions);
        }

        public bool IsAssignableTo(IType assignableToType)
        {
            return this.GetAssignableTypes().Contains(assignableToType);
        }

        public bool IsAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            return pattern != null && this.GetAssignableTypes()
                .Any(type => type.FullNameMatches(pattern, useRegularExpressions));
        }

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Class other)
        {
            return Equals(Type, other.Type) && Equals(IsAbstract, other.IsAbstract) &&
                   Equals(IsSealed, other.IsSealed);
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
                return hashCode;
            }
        }
    }
}