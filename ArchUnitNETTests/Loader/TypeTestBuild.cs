//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Domain.Dependencies.Attributes;
using ArchUnitNETTests.Domain.Dependencies.Members;
using ArchUnitType = ArchUnitNET.Loader.Type;

namespace ArchUnitNETTests.Loader
{
    public static class TypeTestBuild
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildTypeTestData(Type originType)
        {
            var clazz = Architecture.GetITypeOfType(originType);
            clazz.RequiredNotNull();

            var type = new ArchUnitType(
                clazz.FullName,
                clazz.Name,
                clazz.Assembly,
                clazz.Namespace,
                clazz.Visibility,
                clazz.IsNested,
                clazz.IsGeneric,
                clazz.IsStub,
                clazz.IsCompilerGenerated
            );

            type.GenericParameters.AddRange(clazz.GenericParameters);

            return new object[] { type };
        }

        private static object[] BuildTypeEquivalenceTestData(Type originType)
        {
            var clazz = Architecture.GetITypeOfType(originType);
            clazz.RequiredNotNull();
            var type = new ArchUnitType(
                clazz.FullName,
                clazz.Name,
                clazz.Assembly,
                clazz.Namespace,
                clazz.Visibility,
                clazz.IsNested,
                clazz.IsGeneric,
                clazz.IsStub,
                clazz.IsCompilerGenerated
            );
            object duplicateType = new ArchUnitType(
                clazz.FullName,
                clazz.Name,
                clazz.Assembly,
                clazz.Namespace,
                clazz.Visibility,
                clazz.IsNested,
                clazz.IsGeneric,
                clazz.IsStub,
                clazz.IsCompilerGenerated
            );
            var typeCopy = type;
            object referenceCopy = type;

            return new[] { type, duplicateType, typeCopy, referenceCopy };
        }

        public class TypeModelingTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _typeModelingData = new List<object[]>
            {
                BuildTypeTestData(typeof(ClassWithFieldA)),
                BuildTypeTestData(typeof(ClassWithPropertyA)),
                BuildTypeTestData(typeof(ClassWithMethodA)),
                BuildTypeTestData(typeof(ClassWithExampleAttribute)),
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
                BuildTypeEquivalenceTestData(typeof(ClassWithExampleAttribute)),
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
