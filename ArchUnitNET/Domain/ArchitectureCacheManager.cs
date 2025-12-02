using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Enhanced caching manager for Architecture instances with automatic invalidation support
    /// </summary>
    public class ArchitectureCacheManager
    {
        private readonly ConcurrentDictionary<EnhancedCacheKey, CachedArchitecture> _cache =
            new ConcurrentDictionary<EnhancedCacheKey, CachedArchitecture>();

        private static readonly Lazy<ArchitectureCacheManager> _instance =
            new Lazy<ArchitectureCacheManager>(() => new ArchitectureCacheManager());

        private static readonly string ArchUnitNetVersion =
            typeof(Architecture).Assembly.GetName().Version?.ToString() ?? "unknown";

        protected ArchitectureCacheManager() { }

        public static ArchitectureCacheManager Instance => _instance.Value;

        /// <summary>
        /// Try to get a cached architecture. Returns null if not found or if cache is invalid.
        /// </summary>
        public Architecture TryGetArchitecture(
            ArchitectureCacheKey baseCacheKey,
            IEnumerable<AssemblyMetadata> assemblyMetadata,
            ArchLoaderCacheConfig config)
        {
            if (config == null || !config.CachingEnabled)
            {
                return null;
            }

            var assemblyMetadatas = assemblyMetadata as AssemblyMetadata[] ?? assemblyMetadata.ToArray();
            var enhancedKey = new EnhancedCacheKey(
                baseCacheKey,
                config.UseFileBasedInvalidation ? assemblyMetadatas : null,
                config.UserCacheKey,
                config.IncludeVersionInCacheKey ? ArchUnitNetVersion : null
            );

            if (_cache.TryGetValue(enhancedKey, out var cached))
            {
                if (config.UseFileBasedInvalidation && cached.AssemblyMetadata != null)
                {
                    var currentMetadata = assemblyMetadatas?.ToList();
                    if (!AreAssembliesUnchanged(cached.AssemblyMetadata, currentMetadata))
                    {
                        _cache.TryRemove(enhancedKey, out _);
                        return null;
                    }
                }

                return cached.Architecture;
            }

            return null;
        }

        /// <summary>
        /// Add an architecture to the cache
        /// </summary>
        public bool Add(
            ArchitectureCacheKey baseCacheKey,
            Architecture architecture,
            IEnumerable<AssemblyMetadata> assemblyMetadata,
            ArchLoaderCacheConfig config)
        {
            if (config == null || !config.CachingEnabled)
            {
                return false;
            }

            var assemblyMetadatas = assemblyMetadata as AssemblyMetadata[] ?? assemblyMetadata.ToArray();
            var enhancedKey = new EnhancedCacheKey(
                baseCacheKey,
                config.UseFileBasedInvalidation ? assemblyMetadatas : null,
                config.UserCacheKey,
                config.IncludeVersionInCacheKey ? ArchUnitNetVersion : null
            );

            var cached = new CachedArchitecture
            {
                Architecture = architecture,
                AssemblyMetadata = config.UseFileBasedInvalidation
                    ? assemblyMetadatas?.ToList()
                    : null,
                CachedAt = DateTime.UtcNow
            };

            return _cache.TryAdd(enhancedKey, cached);
        }

        /// <summary>
        /// Clear all cached architectures
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// Get the number of cached architectures
        /// </summary>
        public int Count => _cache.Count;

        private static bool AreAssembliesUnchanged(
            List<AssemblyMetadata> cached,
            List<AssemblyMetadata> current)
        {
            if (cached == null || current == null)
                return cached == current;

            if (cached.Count != current.Count)
                return false;

            var cachedDict = cached.ToDictionary(m => m.FilePath, StringComparer.OrdinalIgnoreCase);

            foreach (var currentMeta in current)
            {
                if (!cachedDict.TryGetValue(currentMeta.FilePath, out var cachedMeta))
                    return false;

                if (!cachedMeta.Equals(currentMeta))
                    return false;
            }

            return true;
        }

        private class CachedArchitecture
        {
            public Architecture Architecture { get; set; }
            public List<AssemblyMetadata> AssemblyMetadata { get; set; }
            public DateTime CachedAt { get; set; }
        }

        private class EnhancedCacheKey : IEquatable<EnhancedCacheKey>
        {
            private readonly ArchitectureCacheKey _baseCacheKey;
            private readonly List<AssemblyMetadata> _assemblyMetadata;
            private readonly string _userCacheKey;
            private readonly string _version;
            private readonly int _hashCode;

            public EnhancedCacheKey(
                ArchitectureCacheKey baseCacheKey,
                IEnumerable<AssemblyMetadata> assemblyMetadata,
                string userCacheKey,
                string version)
            {
                _baseCacheKey = baseCacheKey ?? throw new ArgumentNullException(nameof(baseCacheKey));
                _assemblyMetadata = assemblyMetadata?.OrderBy(m => m.FilePath, StringComparer.OrdinalIgnoreCase).ToList();
                _userCacheKey = userCacheKey;
                _version = version;
                _hashCode = ComputeHashCode();
            }

            private int ComputeHashCode()
            {
                unchecked
                {
                    var hash = _baseCacheKey.GetHashCode();
                    hash = (hash * 397) ^ (_userCacheKey?.GetHashCode() ?? 0);
                    hash = (hash * 397) ^ (_version?.GetHashCode() ?? 0);

                    if (_assemblyMetadata != null)
                    {
                        foreach (var metadata in _assemblyMetadata)
                        {
                            hash = (hash * 397) ^ metadata.GetHashCode();
                        }
                    }

                    return hash;
                }
            }

            public bool Equals(EnhancedCacheKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                if (!_baseCacheKey.Equals(other._baseCacheKey))
                    return false;

                if (_userCacheKey != other._userCacheKey)
                    return false;

                if (_version != other._version)
                    return false;

                if (_assemblyMetadata == null && other._assemblyMetadata == null)
                    return true;

                if (_assemblyMetadata == null || other._assemblyMetadata == null)
                    return false;

                if (_assemblyMetadata.Count != other._assemblyMetadata.Count)
                    return false;

                return _assemblyMetadata.SequenceEqual(other._assemblyMetadata);
            }

            public override bool Equals(object obj)
            {
                return obj is EnhancedCacheKey other && Equals(other);
            }

            public override int GetHashCode() => _hashCode;
        }
    }
}
