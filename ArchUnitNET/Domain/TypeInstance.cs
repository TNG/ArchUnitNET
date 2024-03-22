//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class TypeInstance<T> : ITypeInstance<T>
        where T : IType
    {
        public TypeInstance(
            T type,
            IEnumerable<GenericArgument> genericArguments,
            IEnumerable<int> arrayDimensions
        )
        {
            Type = type;
            GenericArguments = genericArguments;
            ArrayDimensions = arrayDimensions;
            IsArray = ArrayDimensions.Any();
        }

        public TypeInstance(T type, IEnumerable<GenericArgument> genericArguments)
            : this(type, genericArguments, Enumerable.Empty<int>()) { }

        public TypeInstance(T type)
            : this(type, Enumerable.Empty<GenericArgument>()) { }

        public T Type { get; }
        public IEnumerable<GenericArgument> GenericArguments { get; }
        public bool IsArray { get; }
        public IEnumerable<int> ArrayDimensions { get; }

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

            return obj.GetType() == GetType() && Equals((TypeInstance<T>)obj);
        }

        private bool Equals(TypeInstance<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Type, other.Type)
                && GenericArguments.SequenceEqual(other.GenericArguments)
                && Equals(IsArray, other.IsArray)
                && ArrayDimensions.SequenceEqual(other.ArrayDimensions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = GenericArguments.Aggregate(
                    hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0)
                );
                hashCode = (hashCode * 397) ^ IsArray.GetHashCode();
                hashCode = ArrayDimensions.Aggregate(
                    hashCode,
                    (current, dim) => (current * 397) ^ dim.GetHashCode()
                );
                return hashCode;
            }
        }
    }
}
