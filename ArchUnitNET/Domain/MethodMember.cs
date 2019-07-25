/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Domain
{
    public class MethodMember : IMember
    {
        public MethodMember(string name, string fullName, IType declaringType, Visibility visibility,
            List<IType> parameters, IType returnType, bool isVirtual, MethodForm methodForm)
        {
            Name = name;
            FullName = fullName;
            DeclaringType = declaringType;
            Visibility = visibility;
            Parameters = parameters;
            ReturnType = returnType;
            IsVirtual = isVirtual;
            MethodForm = methodForm;
        }

        public bool IsVirtual { get; }
        public MethodForm MethodForm { get; }
        public Visibility Visibility { get; }
        public List<IType> Parameters { get; }
        public IType ReturnType { get; }
        public List<Attribute> Attributes { get; } = new List<Attribute>();
        public List<IMemberTypeDependency> MemberDependencies { get; } = new List<IMemberTypeDependency>();
        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } = new List<IMemberTypeDependency>();
        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();
        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();
        public string Name { get; }
        public string FullName { get; }
        public IType DeclaringType { get; }

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
                   && Equals(DeclaringType, other.DeclaringType);
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
                return hashCode;
            }
        }
    }
}