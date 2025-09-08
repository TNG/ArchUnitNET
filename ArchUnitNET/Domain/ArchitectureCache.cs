using System.Runtime.Caching;

namespace ArchUnitNET.Domain
{
    public class ArchitectureCache
    {
        protected readonly MemoryCache _cache = new MemoryCache(nameof(ArchitectureCache));

        protected ArchitectureCache() { }

        public static ArchitectureCache Instance { get; } = new ArchitectureCache();

        public Architecture TryGetArchitecture(ArchitectureCacheKey architectureCacheKey)
        {
            return _cache.Get(architectureCacheKey.ToString()) as Architecture;
        }

        public void Add(ArchitectureCacheKey architectureCacheKey, Architecture architecture)
        {
            _cache.Add(architectureCacheKey.ToString(), architecture, new CacheItemPolicy());
        }
    }
}
