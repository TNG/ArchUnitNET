/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class ArchitectureCacheKey : IEquatable<ArchitectureCacheKey>
    {
        private readonly SortedSet<(string moduleName, string filter)> _architectureCacheKey =
            new SortedSet<(string moduleName, string filter)>(new ArchitectureCacheKeyComparer());

        public void Add(string moduleName, string filter)
        {
            _architectureCacheKey.Add((moduleName, filter));
        }

        public bool Equals(ArchitectureCacheKey other)
        {
            return other != null && _architectureCacheKey.SequenceEqual(other._architectureCacheKey);
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

            return obj.GetType() == GetType() && Equals((ArchitectureCacheKey) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 397;
            _architectureCacheKey.ForEach(tuple => { hashCode = (hashCode * 131) ^ tuple.GetHashCode(); });
            return hashCode;
        }
    }

    internal class ArchitectureCacheKeyComparer : IComparer<(string moduleName, string filter)>
    {
        public int Compare((string moduleName, string filter) x,
            (string moduleName, string filter) y)
        {
            var moduleNameComparison = string.Compare(x.moduleName, y.moduleName, StringComparison.Ordinal);
            return moduleNameComparison == 0
                ? string.Compare(x.filter, y.filter, StringComparison.Ordinal)
                : moduleNameComparison;
        }
    }
}