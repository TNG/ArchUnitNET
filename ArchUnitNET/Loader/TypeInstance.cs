//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader
{
    public class TypeInstance<T> where T : IType
    {
        public TypeInstance(T type, IEnumerable<GenericArgument> genericArguments)
        {
            Type = type;
            GenericArguments = genericArguments;
        }

        public TypeInstance(T type)
        {
            Type = type;
            GenericArguments = Enumerable.Empty<GenericArgument>();
        }

        public T Type { get; }
        public IEnumerable<GenericArgument> GenericArguments { get; }

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

            return obj.GetType() == GetType() && Equals((TypeInstance<T>) obj);
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

            return Equals(Type, other.Type) && GenericArguments.SequenceEqual(other.GenericArguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = GenericArguments.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                return hashCode;
            }
        }
    }
}