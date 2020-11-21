//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain
{
    public class MethodMember : IMember
    {
        private readonly IEnumerable<TypeInstance<IType>> _parameterInstances;
        private readonly TypeInstance<IType> _returnTypeInstance;

        public MethodMember(string name, string fullName, IType declaringType, Visibility visibility,
            IEnumerable<TypeInstance<IType>> parameters, TypeInstance<IType> returnType, bool isVirtual,
            MethodForm methodForm,
            bool isGeneric)
        {
            Name = name;
            FullName = fullName;
            DeclaringType = declaringType;
            Visibility = visibility;
            _parameterInstances = parameters;
            _returnTypeInstance = returnType;
            IsVirtual = isVirtual;
            MethodForm = methodForm;
            IsGeneric = isGeneric;
            GenericParameters = new List<GenericParameter>();
        }

        public bool IsVirtual { get; }
        public MethodForm MethodForm { get; }
        public IEnumerable<IType> Parameters => _parameterInstances.Select(instance => instance.Type);
        public IType ReturnType => _returnTypeInstance.Type;
        public bool IsGeneric { get; }
        public List<GenericParameter> GenericParameters { get; }
        public Visibility Visibility { get; }
        public List<Attribute> Attributes { get; } = new List<Attribute>();
        public List<IMemberTypeDependency> MemberDependencies { get; } = new List<IMemberTypeDependency>();
        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } = new List<IMemberTypeDependency>();
        public List<ITypeDependency> Dependencies => MemberDependencies.Cast<ITypeDependency>().ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            MemberBackwardsDependencies.Cast<ITypeDependency>().ToList();

        public string Name { get; }
        public string FullName { get; }
        public IType DeclaringType { get; }

        public override string ToString()
        {
            return $"{DeclaringType.FullName}::{Name}";
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

            return obj.GetType() == GetType() && Equals((MethodMember) obj);
        }

        private bool Equals(MethodMember other)
        {
            return IsVirtual == other.IsVirtual && MethodForm == other.MethodForm && Visibility == other.Visibility
                   && Equals(Parameters, other.Parameters) && Equals(ReturnType, other.ReturnType)
                   && string.Equals(Name, other.Name) && string.Equals(FullName, other.FullName)
                   && Equals(DeclaringType, other.DeclaringType) && Equals(IsGeneric, other.IsGeneric);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsVirtual.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) MethodForm;
                hashCode = (hashCode * 397) ^ (int) Visibility;
                hashCode = (hashCode * 397) ^ (Parameters != null ? Parameters.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ReturnType != null ? ReturnType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsGeneric.GetHashCode();
                return hashCode;
            }
        }
    }
}