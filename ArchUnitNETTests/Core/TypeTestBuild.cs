/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;

namespace ArchUnitNETTests.Core
{
    public static class TypeTestBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildTypeTestData(Type originType)
        {
            var clazz = Architecture.GetITypeOfType(originType);
            clazz.RequiredNotNull();

            var type = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace,
                clazz.Visibility, clazz.IsNested);

            return new object[] {type};
        }

        private static object[] BuildTypeEquivalenceTestData(Type originType)
        {
            var clazz = Architecture.GetITypeOfType(originType);
            clazz.RequiredNotNull();
            var type = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace,
                clazz.Visibility, clazz.IsNested);
            object duplicateType = new ArchUnitNET.Core.Type(clazz.FullName, clazz.Name, clazz.Assembly,
                clazz.Namespace, clazz.Visibility, clazz.IsNested);
            var typeCopy = type;
            object referenceCopy = type;

            return new[] {type, duplicateType, typeCopy, referenceCopy};
        }

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
    }
}