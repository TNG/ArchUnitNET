using System.Collections.Concurrent;

namespace ArchUnitNET.Domain
{
    public class ArchitectureCache
    {
        protected readonly ConcurrentDictionary<ArchitectureCacheKey, Architecture> Cache =
            new ConcurrentDictionary<ArchitectureCacheKey, Architecture>();

        protected ArchitectureCache() { }

        public static ArchitectureCache Instance { get; } = new ArchitectureCache();

        public Architecture TryGetArchitecture(ArchitectureCacheKey architectureCacheKey)
        {
            return Cache.TryGetValue(architectureCacheKey, out var matchArchitecture)
                ? matchArchitecture
                : null;
        }

        public bool Add(ArchitectureCacheKey architectureCacheKey, Architecture architecture)
        {
            return Cache.TryAdd(architectureCacheKey, architecture);
        }

        public void Clear() => Cache.Clear();
    }
}
