using System;
using System.Collections.Generic;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class ObjectProviderCache
    {
        private readonly Architecture _architecture;
        private readonly Dictionary<dynamic, dynamic> _cache;

        public ObjectProviderCache(Architecture architecture)
        {
            _architecture = architecture;
            _cache = new Dictionary<dynamic, dynamic>();
        }

        public IEnumerable<T> GetOrCreateObjects<T>(IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction) where T : ICanBeAnalyzed
        {
            if (!_cache.ContainsKey(objectProvider))
            {
                _cache.Add(objectProvider, providingFunction(_architecture));
            }

            return _cache[objectProvider];
        }
    }
}