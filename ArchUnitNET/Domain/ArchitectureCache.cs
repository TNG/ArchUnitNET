//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

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
