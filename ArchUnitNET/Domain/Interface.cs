//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain
{
    public class Interface : IType
    {
        public Interface(IType type)
        {
            Type = type;
        }

        public IType Type { get; }
        public string Name => Type.Name;
        public string FullName => Type.FullName;

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public List<Attribute> Attributes { get; } = new List<Attribute>();

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;
        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;

        public MemberList Members => Type.Members;
        public List<IType> GenericTypeParameters => Type.GenericTypeParameters;
        public IType GenericType => Type.GenericType;
        public List<IType> GenericTypeArguments => Type.GenericTypeArguments;

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
            if (assignableToType is Interface intf)
            {
                return Equals(intf) || ImplementsInterface(intf);
            }

            return false;
        }

        public bool IsAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            return this.FullNameMatches(pattern, useRegularExpressions) ||
                   ImplementsInterface(pattern, useRegularExpressions);
        }

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Interface other)
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

            return obj.GetType() == GetType() && Equals((Interface) obj);
        }

        public override int GetHashCode()
        {
            return Type != null ? Type.GetHashCode() : 0;
        }
    }
}