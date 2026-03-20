using System.Collections.Concurrent;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// A singleton cache that stores <see cref="Architecture"/> instances keyed by
    /// <see cref="ArchitectureCacheKey"/>. This avoids re-loading and re-analyzing
    /// assemblies when the same set of assemblies is loaded multiple times (e.g., across
    /// multiple test classes that share the same architecture).
    /// </summary>
    /// <remarks>
    /// The architecture cache operates at the assembly-loading level: it caches the fully
    /// constructed <see cref="Architecture"/> object. This is separate from rule evaluation
    /// caching, which caches individual rule evaluation results within an architecture.
    /// Use <see cref="ArchLoader.WithoutArchitectureCache"/> on <c>ArchLoader</c> to bypass this
    /// cache when building an architecture.
    /// </remarks>
    public class ArchitectureCache
    {
        protected readonly ConcurrentDictionary<ArchitectureCacheKey, Architecture> Cache =
            new ConcurrentDictionary<ArchitectureCacheKey, Architecture>();

        protected ArchitectureCache() { }

        /// <summary>
        /// Gets the singleton instance of the architecture cache.
        /// </summary>
        public static ArchitectureCache Instance { get; } = new ArchitectureCache();

        /// <summary>
        /// Attempts to retrieve a cached architecture for the given key.
        /// </summary>
        /// <param name="architectureCacheKey">The key identifying the architecture.</param>
        /// <returns>The cached architecture, or <c>null</c> if not found.</returns>
        public Architecture TryGetArchitecture(ArchitectureCacheKey architectureCacheKey)
        {
            return Cache.TryGetValue(architectureCacheKey, out var matchArchitecture)
                ? matchArchitecture
                : null;
        }

        /// <summary>
        /// Adds an architecture to the cache. If the key already exists, the existing entry is kept.
        /// </summary>
        /// <param name="architectureCacheKey">The key identifying the architecture.</param>
        /// <param name="architecture">The architecture to cache.</param>
        /// <returns><c>true</c> if the architecture was added; <c>false</c> if the key already existed.</returns>
        public bool Add(ArchitectureCacheKey architectureCacheKey, Architecture architecture)
        {
            return Cache.TryAdd(architectureCacheKey, architecture);
        }

        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        public void Clear() => Cache.Clear();
    }
}
