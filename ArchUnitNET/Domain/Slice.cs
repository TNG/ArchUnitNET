//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class Slice<TKey> : IHasDependencies
    {
        private readonly List<IType> _types;

        public Slice(TKey sliceKey, List<IType> types)
        {
            _types = types;
            SliceKey = sliceKey;
        }

        public TKey SliceKey { get; }
        public IEnumerable<IType> Types => _types;
        public IEnumerable<Class> Classes => _types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => _types.OfType<Interface>();


        public List<ITypeDependency> Dependencies => _types.SelectMany(type => type.Dependencies).ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            _types.SelectMany(type => type.BackwardsDependencies).ToList();

        public bool Equals(Slice<TKey> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(_types, other._types) && Equals(SliceKey, other.SliceKey);
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

            return Equals((Slice<TKey>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _types != null ? _types.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (SliceKey != null ? SliceKey.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}