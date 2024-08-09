//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class ClassDependenciesIncludeMemberDependencies
    {
        [Theory]
        [ClassData(
            typeof(ClassDependenciesIncludeMemberDependenciesBuild.MethodDependenciesWithClassTestData)
        )]
        public void DependenciesOfMembersAreDependenciesOfDeclaringType(Class clazz)
        {
            var methodMembers = clazz.GetMethodMembers();
            methodMembers.ForEach(methodMember =>
                Assert.True(
                    clazz.HasDependencies(
                        methodMember.MemberDependencies.Where(dependency =>
                            // E.g. static members with initializer lead to a dependency
                            // to the declaring type which we want to ignore
                            // https://github.com/TNG/ArchUnitNET/issues/229
                            dependency.Target.FullName != clazz.FullName
                        )
                    )
                )
            );
        }
    }

    public static class ClassDependenciesIncludeMemberDependenciesBuild
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        internal static object[] BuildClassTestData(Type originType)
        {
            var originClass = Architecture.GetClassOfType(originType);
            return new object[] { originClass };
        }

        public class MethodDependenciesWithClassTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodCallDependencyData = new List<object[]>
            {
                BuildClassTestData(typeof(ClassWithMethodA)),
                BuildClassTestData(typeof(ClassWithMethodA)),
                BuildClassTestData(typeof(ClassWithConstructors)),
                BuildClassTestData(typeof(ClassWithMethodSignatureA)),
                BuildClassTestData(typeof(ClassWithMethodSignatureB)),
                BuildClassTestData(typeof(ClassWithMethodSignatureC))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _methodCallDependencyData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
