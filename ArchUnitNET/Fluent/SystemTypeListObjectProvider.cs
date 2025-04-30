using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Fluent
{
    public class SystemTypeListObjectProvider<T> : ISizedObjectProvider<T>
        where T : IType
    {
        private readonly List<Type> _types;

        public SystemTypeListObjectProvider(List<Type> types)
        {
            _types = types;
            Description = string.Join(" or ", types.Select(type => $"\"{type.FullName}\""));
        }

        public string Description { get; }

        public int Count => _types.Count;

        public IEnumerable<T> GetObjects(Architecture architecture)
        {
            return _types
                .Select(architecture.GetITypeOfType)
                .Select(
                    (type) =>
                    {
                        if (!(type is T result))
                        {
                            throw new ArgumentException($"Type {type} is not of type {typeof(T)}");
                        }
                        return result;
                    }
                );
        }

        private bool Equals(SystemTypeListObjectProvider<T> other)
        {
            return string.Equals(Description, other.Description);
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

            return obj.GetType() == GetType() && Equals((SystemTypeListObjectProvider<T>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
