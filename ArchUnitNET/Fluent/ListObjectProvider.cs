using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ListObjectProvider<T> : ISizedObjectProvider<T>
        where T : ICanBeAnalyzed
    {
        private readonly IEnumerable<T> _objects;

        public ListObjectProvider(List<T> objects)
        {
            _objects = objects;
            Description = string.Join(" or ", objects.Select(obj => $"\"{obj.FullName}\""));
        }

        public string Description { get; }

        public int Count => _objects.Count();

        public IEnumerable<T> GetObjects(Architecture architecture)
        {
            return _objects;
        }

        private bool Equals(ListObjectProvider<T> other)
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

            return obj.GetType() == GetType() && Equals((ListObjectProvider<T>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
