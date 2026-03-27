using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Identifies a cached <see cref="Architecture"/> by the set of loaded modules,
    /// their namespace filters, and whether rule evaluation caching is disabled.
    /// Two keys are equal when they represent the same combination of modules, filters,
    /// and caching flag, regardless of insertion order.
    /// </summary>
    public class ArchitectureCacheKey : IEquatable<ArchitectureCacheKey>
    {
        private readonly SortedSet<(string moduleName, string filter)> _architectureCacheKey =
            new SortedSet<(string moduleName, string filter)>(new ArchitectureCacheKeyComparer());

        private bool _ruleEvaluationCacheDisabled;

        public bool Equals(ArchitectureCacheKey other)
        {
            return other != null
                && _ruleEvaluationCacheDisabled == other._ruleEvaluationCacheDisabled
                && _architectureCacheKey.SequenceEqual(other._architectureCacheKey);
        }

        /// <summary>
        /// Adds a module and optional namespace filter to this key.
        /// </summary>
        /// <param name="moduleName">The name of the loaded module.</param>
        /// <param name="filter">
        /// The namespace filter applied when loading, or <c>null</c> if no filter was used.
        /// </param>
        public void Add(string moduleName, string filter)
        {
            _architectureCacheKey.Add((moduleName, filter));
        }

        /// <summary>
        /// Marks this key as representing an architecture with rule evaluation caching disabled.
        /// Architectures with caching disabled are stored separately in the cache.
        /// </summary>
        public void SetRuleEvaluationCacheDisabled()
        {
            _ruleEvaluationCacheDisabled = true;
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

            return obj.GetType() == GetType() && Equals((ArchitectureCacheKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397;
                _architectureCacheKey.ForEach(tuple =>
                {
                    hashCode = (hashCode * 131) ^ tuple.GetHashCode();
                });
                hashCode = (hashCode * 131) ^ _ruleEvaluationCacheDisabled.GetHashCode();
                return hashCode;
            }
        }
    }

    internal class ArchitectureCacheKeyComparer : IComparer<(string moduleName, string filter)>
    {
        public int Compare(
            (string moduleName, string filter) x,
            (string moduleName, string filter) y
        )
        {
            var moduleNameComparison = string.Compare(
                x.moduleName,
                y.moduleName,
                StringComparison.Ordinal
            );
            return moduleNameComparison == 0
                ? string.Compare(x.filter, y.filter, StringComparison.Ordinal)
                : moduleNameComparison;
        }
    }
}
