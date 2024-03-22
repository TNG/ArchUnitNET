//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class AttributeArgument : ITypeInstance<IType>
    {
        private readonly ITypeInstance<IType> _typeInstance;

        public AttributeArgument(object value, ITypeInstance<IType> typeInstance)
        {
            Value = value;
            _typeInstance = typeInstance;
        }

        public readonly object Value;

        public IType Type => _typeInstance.Type;
        public IEnumerable<GenericArgument> GenericArguments => _typeInstance.GenericArguments;
        public bool IsArray => _typeInstance.IsArray;
        public IEnumerable<int> ArrayDimensions => _typeInstance.ArrayDimensions;

        private bool Equals(AttributeArgument other)
        {
            return Equals(_typeInstance, other._typeInstance) && Equals(Value, other.Value);
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

            return obj.GetType() == GetType() && Equals((AttributeArgument)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _typeInstance != null ? Type.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
