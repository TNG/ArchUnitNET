using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain
{
    public class ArchitectureCacheKey
    {
        private readonly SortedSet<(string moduleName, string filter)> _architectureCacheKey =
            new SortedSet<(string moduleName, string filter)>(new ArchitectureCacheKeyComparer());

        public void Add(string moduleName, string filter)
        {
            _architectureCacheKey.Add((moduleName, filter));
        }

        public override string ToString()
        {
            return string.Join(",", _architectureCacheKey);
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
