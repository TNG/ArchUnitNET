using System;
using System.Collections.Generic;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class ObjectProviderCache
    {
        private readonly Architecture _architecture;
        private readonly Dictionary<int, object> _cache;

        public ObjectProviderCache(Architecture architecture)
        {
            _architecture = architecture;
            _cache = new Dictionary<int, object>();
        }

        public IEnumerable<T> GetOrCreateObjects<T>(IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction) where T : ICanBeAnalyzed
        {
            unchecked
            {
                var key = (objectProvider.GetHashCode() * 397) ^ objectProvider.GetType().GetHashCode();
                if (!_cache.ContainsKey(key))
                {
                    _cache.Add(key, providingFunction(_architecture));
                }

                return (IEnumerable<T>) _cache[key];
            }
        }
    }
}