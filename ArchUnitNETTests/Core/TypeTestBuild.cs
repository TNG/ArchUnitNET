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
using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent;

namespace ArchUnitNETTests.Core
{
    public static class TypeTestBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        public class TypeModelingTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _typeModelingData = new List<object[]>
            {
                BuildTypeTestData(typeof(ClassWithFieldA)),
                BuildTypeTestData(typeof(ClassWithPropertyA)),
                BuildTypeTestData(typeof(ClassWithMethodA)),
                BuildTypeTestData(typeof(ClassWithExampleAttribute))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _typeModelingData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static object[] BuildTypeTestData(Type originType)
        {
            var clazz = Architecture.GetTypeOfType(originType);
            clazz.RequiredNotNull();

            var type = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace);

            return new object[] {type};
        }
        
        public class TypeEquivalencyModelingTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _typeModelingData = new List<object[]>
            {
                BuildTypeEquivalenceTestData(typeof(ClassWithFieldA)),
                BuildTypeEquivalenceTestData(typeof(ClassWithPropertyA)),
                BuildTypeEquivalenceTestData(typeof(ClassWithMethodA)),
                BuildTypeEquivalenceTestData(typeof(ClassWithExampleAttribute))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _typeModelingData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static object[] BuildTypeEquivalenceTestData(Type originType)
        {
            var clazz = Architecture.GetTypeOfType(originType);
            clazz.RequiredNotNull();
            var type = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace);
            object duplicateType = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly,
                clazz.Namespace);
            var typeCopy = type;
            object referenceCopy = type;

            return new[] {type, duplicateType, typeCopy, referenceCopy};
        }
    }
}