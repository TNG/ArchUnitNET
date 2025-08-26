using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    internal class ObjectProvider<T> : ISizedObjectProvider<T>
        where T : ICanBeAnalyzed
    {
        private readonly List<T> _objects;

        public ObjectProvider()
            : this(new List<T>()) { }

        public ObjectProvider(IEnumerable<T> objects)
        {
            _objects = objects.ToList();
            Description = string.Join(" or ", _objects.Select(obj => $"\"{obj.FullName}\""));
        }

        public string Description { get; }

        public int Count => _objects.Count;

        public IEnumerable<T> GetObjects(Architecture architecture)
        {
            return _objects;
        }

        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        )
        {
            switch (Count)
            {
                case 0:
                    return emptyDescription;
                case 1:
                    return $"{singleDescription} {Description}";
            }
            return $"{multipleDescription} {Description}";
        }

        private bool Equals(ObjectProvider<T> other)
        {
            return _objects.SequenceEqual(other._objects);
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

            return obj.GetType() == GetType() && Equals((ObjectProvider<T>)obj);
        }

        public override int GetHashCode()
        {
            return _objects != null
                ? _objects.Aggregate(
                    0,
                    (current, obj) => (current * 397) ^ (obj?.GetHashCode() ?? 0)
                )
                : 0;
        }
    }
}
