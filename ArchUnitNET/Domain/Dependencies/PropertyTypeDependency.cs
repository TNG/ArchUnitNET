//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public class PropertyTypeDependency : IMemberTypeDependency
    {
        private readonly PropertyMember _originMember;

        public PropertyTypeDependency(PropertyMember property)
        {
            _originMember = property;
        }

        public IType Target => _originMember.Type;
        public IMember OriginMember => _originMember;

        public IType Origin => _originMember.DeclaringType;
        public IEnumerable<GenericArgument> TargetGenericArguments => _originMember.TypeGenericArguments;

        public bool Equals(PropertyTypeDependency other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(_originMember, other._originMember);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((PropertyTypeDependency) obj);
        }

        public override int GetHashCode()
        {
            return _originMember != null ? _originMember.GetHashCode() : 0;
        }
    }
}