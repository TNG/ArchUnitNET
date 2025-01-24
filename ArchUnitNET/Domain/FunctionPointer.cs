// Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class FunctionPointer : IType
    {
        private readonly IType _type;

        public FunctionPointer(
            IType type,
            ITypeInstance<IType> returnTypeInstance,
            List<ITypeInstance<IType>> parameterTypeInstances
        )
        {
            _type = type;
            ReturnTypeInstance = returnTypeInstance;
            ParameterTypeInstances = parameterTypeInstances;
        }

        public Namespace Namespace => _type.Namespace;
        public Assembly Assembly => _type.Assembly;
        public MemberList Members => _type.Members;
        public IEnumerable<IType> ImplementedInterfaces => _type.ImplementedInterfaces;
        public bool IsNested => _type.IsNested;
        public bool IsStub => _type.IsStub;
        public bool IsGenericParameter => _type.IsGenericParameter;
        public string Name => _type.Name;
        public string FullName => _type.FullName;
        public string AssemblyQualifiedName => _type.AssemblyQualifiedName;
        public Visibility Visibility => _type.Visibility;
        public bool IsGeneric => _type.IsGeneric;
        public List<GenericParameter> GenericParameters => _type.GenericParameters;
        public bool IsCompilerGenerated => _type.IsCompilerGenerated;
        public List<ITypeDependency> Dependencies => _type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => _type.BackwardsDependencies;
        public IEnumerable<Attribute> Attributes => _type.Attributes;
        public List<AttributeInstance> AttributeInstances => _type.AttributeInstances;
        public ITypeInstance<IType> ReturnTypeInstance { get; }
        public List<ITypeInstance<IType>> ParameterTypeInstances { get; }

        public bool Equals(FunctionPointer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(_type, other._type)
                && Equals(ReturnTypeInstance, other.ReturnTypeInstance)
                && ParameterTypeInstances.SequenceEqual(other.ParameterTypeInstances);
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
            return obj.GetType() == GetType() && Equals((FunctionPointer)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _type.GetHashCode();
                hashCode =
                    (hashCode * 397)
                    ^ (ReturnTypeInstance != null ? ReturnTypeInstance.GetHashCode() : 0);
                hashCode = ParameterTypeInstances.Aggregate(
                    hashCode,
                    (current, typeInstance) =>
                        (current * 397) ^ (typeInstance != null ? typeInstance.GetHashCode() : 0)
                );
                return hashCode;
            }
        }
    }
}
