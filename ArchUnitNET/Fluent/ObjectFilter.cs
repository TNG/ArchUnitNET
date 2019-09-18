using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ObjectFilter<T> : IHasDescription where T : ICanBeAnalyzed
    {
        private readonly Func<T, bool> _filter;

        public ObjectFilter(Func<T, bool> filter, string description)
        {
            _filter = filter;
            Description = description;
        }

        public string Description { get; }

        public bool CheckFilter(T obj)
        {
            return _filter(obj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}