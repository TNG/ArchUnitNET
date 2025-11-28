namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Configuration options for the ArchLoader caching mechanism
    /// </summary>
    public sealed class ArchLoaderCacheConfig
    {
        /// <summary>
        /// Creates a new instance with default settings (caching enabled, file-based invalidation enabled)
        /// </summary>
        public ArchLoaderCacheConfig()
        {
        }

        /// <summary>
        /// Gets or sets whether caching is enabled. Default is true.
        /// </summary>
        public bool CachingEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to use file-based invalidation (hash + timestamp + size checking).
        /// Default is true. When false, only module names are used for cache keys.
        /// </summary>
        public bool UseFileBasedInvalidation { get; set; } = true;

        /// <summary>
        /// Gets or sets an optional user-provided cache key for fine-grained control.
        /// When set, this key is included in the cache key computation.
        /// </summary>
        public string UserCacheKey { get; set; }

        /// <summary>
        /// Gets or sets whether to include the ArchUnitNET version in cache invalidation.
        /// Default is true.
        /// </summary>
        public bool IncludeVersionInCacheKey { get; set; } = true;

        /// <summary>
        /// Creates a copy of this configuration
        /// </summary>
        public ArchLoaderCacheConfig Clone()
        {
            return new ArchLoaderCacheConfig
            {
                CachingEnabled = CachingEnabled,
                UseFileBasedInvalidation = UseFileBasedInvalidation,
                UserCacheKey = UserCacheKey,
                IncludeVersionInCacheKey = IncludeVersionInCacheKey
            };
        }
    }
}
