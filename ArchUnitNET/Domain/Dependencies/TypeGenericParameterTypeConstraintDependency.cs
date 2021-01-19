//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;

namespace ArchUnitNET.Domain.Dependencies
{
    public class TypeGenericParameterTypeConstraintDependency : TypeInstanceDependency
    {
        public TypeGenericParameterTypeConstraintDependency(GenericParameter originGenericParameter,
            ITypeInstance<IType> typeConstraintInstance)
            : base(originGenericParameter, typeConstraintInstance)
        {
            if (originGenericParameter.DeclarerIsMethod)
            {
                throw new ArgumentException(
                    "Use MemberGenericParameterTypeConstraintDependency for Generic Parameters of Methods.");
            }

            OriginGenericParameter = originGenericParameter;
        }

        public GenericParameter OriginGenericParameter { get; }

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

            return obj.GetType() == GetType() && Equals((TypeGenericParameterTypeConstraintDependency) obj);
        }

        private bool Equals(TypeGenericParameterTypeConstraintDependency other)
        {
            return Equals(OriginGenericParameter, other.OriginGenericParameter) &&
                   Equals(TargetInstance, other.TargetInstance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginGenericParameter != null ? OriginGenericParameter.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (TargetInstance != null ? TargetInstance.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}