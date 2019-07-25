/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class Attribute : IType
    {
        public Attribute(Class attributeType)
        {
            Type = attributeType;
        }

        public Class Type { get; }
        public bool IsAbstract => Type.IsAbstract;
        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;
        public List<Attribute> Attributes => Type.Attributes;
        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;

        public IEnumerable<ITypeDependency> DependenciesIncludingInherited => BaseClass != null
            ? Type.Dependencies.Concat(BaseClass.DependenciesIncludingInherited).ToList()
            : Type.Dependencies;

        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;
        public MemberList Members => Type.Members;

        public MemberList MembersIncludingInherited =>
            BaseClass != null
                ? new MemberList(Type.Members.Concat(BaseClass.MembersIncludingInherited).ToList())
                : Type.Members;

        public List<IType> GenericTypeParameters => Type.GenericTypeParameters;
        public IType GenericType => Type.GenericType;
        public List<IType> GenericTypeArguments => Type.GenericTypeArguments;


        public Class BaseClass =>
            (Class) Dependencies.OfType<InheritsBaseClassDependency>().FirstOrDefault()?.Target;

        public List<MethodMember> Constructors => Type.GetConstructors().ToList();

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
                case Attribute attribute:
                    return Type.IsAssignableTo(attribute.Type);
                default:
                    return false;
            }
        }

        private bool Equals(Attribute other)
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

            return obj.GetType() == GetType() && Equals((Attribute) obj);
        }

        public override int GetHashCode()
        {
            return Type != null ? Type.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}