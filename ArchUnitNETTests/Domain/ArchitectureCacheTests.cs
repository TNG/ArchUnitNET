/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
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

using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class ArchitectureCacheTests
    {
        private readonly TestArchitectureCache _testArchitectureCache;
        private readonly ArchitectureCacheKey _testArchitectureCacheKey;
        private readonly Architecture _testEmptyArchitecture;

        public ArchitectureCacheTests()
        {
            _testArchitectureCache = new TestArchitectureCache();

            _testArchitectureCacheKey = new ArchitectureCacheKey();
            _testArchitectureCacheKey.Add(typeof(ArchitectureCacheTests).Assembly.FullName, null);

            _testEmptyArchitecture = new Architecture(new List<Assembly>(), new List<Namespace>(), new List<IType>());
        }


        [Fact]
        public void GetExistingArchitecture()
        {
            _testArchitectureCache.Add(_testArchitectureCacheKey, _testEmptyArchitecture);
            Assert.Equal(_testArchitectureCache.TryGetArchitecture(_testArchitectureCacheKey),
                _testEmptyArchitecture);
        }

        [Fact]
        public void DuplicateArchitectureDetected()
        {
            _testArchitectureCache.Add(_testArchitectureCacheKey, _testEmptyArchitecture);
            _testArchitectureCache.Add(_testArchitectureCacheKey, _testEmptyArchitecture);

            Assert.True(_testArchitectureCache.Size() == 1);
        }
    }
}