using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class ObjectProviderCache
    {
        private readonly Architecture _architecture;
        private readonly ConcurrentDictionary<int, object> _cache;

        public ObjectProviderCache(Architecture architecture)
        {
            _architecture = architecture;
            _cache = new ConcurrentDictionary<int, object>();
        }

        public IEnumerable<T> GetOrCreateObjects<T>(
            IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction
        )
            where T : ICanBeAnalyzed
        {
            unchecked
            {
                var key =
                    (objectProvider.GetHashCode() * 397) ^ objectProvider.GetType().GetHashCode();
                return (IEnumerable<T>)_cache.GetOrAdd(key, k => providingFunction(_architecture));
            }
        }
    }
}
