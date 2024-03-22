//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class ObjectProviderCache
    {
        private readonly Architecture _architecture;
        private readonly ConcurrentDictionary<int, object> _cache;

        public ObjectProviderCache(Architecture architecture)
        {
            _architecture = architecture;
            _cache = new ConcurrentDictionary<int, object>();
        }

        public IEnumerable<T> GetOrCreateObjects<T>(
            IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction
        )
            where T : ICanBeAnalyzed
        {
            unchecked
            {
                var key =
                    (objectProvider.GetHashCode() * 397) ^ objectProvider.GetType().GetHashCode();
                return (IEnumerable<T>)_cache.GetOrAdd(key, k => providingFunction(_architecture));
            }
        }
    }
}
