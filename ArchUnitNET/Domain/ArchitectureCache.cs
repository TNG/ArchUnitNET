/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Concurrent;

namespace ArchUnitNET.Domain
{
    public class ArchitectureCache
    {
        public static ArchitectureCache Instance { get; } = new ArchitectureCache();

        protected readonly ConcurrentDictionary<ArchitectureCacheKey, Architecture> Cache =
            new ConcurrentDictionary<ArchitectureCacheKey, Architecture>();

        protected ArchitectureCache()
        {
        }

        public Architecture TryGetArchitecture(ArchitectureCacheKey architectureCacheKey)
        {
            return Cache.TryGetValue(architectureCacheKey, out var matchArchitecture) ? matchArchitecture : null;
        }

        public bool Add(ArchitectureCacheKey architectureCacheKey, Architecture architecture)
        {
            return Cache.TryAdd(architectureCacheKey, architecture);
        }
    }
}