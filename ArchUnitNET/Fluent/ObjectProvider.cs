using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ObjectProvider<T> : IObjectProvider<T> where T : ICanBeAnalyzed
    {
        private readonly Func<Architecture, IEnumerable<T>> _objects;

        public ObjectProvider(Func<Architecture, IEnumerable<T>> objects, string description)
        {
            _objects = objects;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<T> GetObjects(Architecture architecture)
        {
            return _objects(architecture);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ObjectProvider<T> other)
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

            return obj.GetType() == GetType() && Equals((ObjectProvider<T>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}