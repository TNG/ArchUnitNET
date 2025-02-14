using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class BasicObjectProvider<T> : IObjectProvider<T>
        where T : ICanBeAnalyzed
    {
        private readonly Func<Architecture, IEnumerable<T>> _objects;

        public BasicObjectProvider(Func<Architecture, IEnumerable<T>> objects, string description)
        {
            _objects = objects;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<T> GetObjects(Architecture architecture)
        {
            return architecture.GetOrCreateObjects(this, _objects);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(BasicObjectProvider<T> other)
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

            return obj.GetType() == GetType() && Equals((BasicObjectProvider<T>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
