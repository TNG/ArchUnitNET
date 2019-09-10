/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;

namespace ArchUnitNETTests.Dependencies.Members
{
    public static class MethodDependencyTestBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildMethodCallDependencyTestData(Type originType, string nameOfOriginMember,
            Type targetType, string nameOfTargetMember)
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMembersWithName(nameOfOriginMember).Single();
            var targetClass = Architecture.GetClassOfType(targetType);
            var targetMember = targetClass.GetMethodMembersWithName(nameOfTargetMember).Single();
            var expectedDependency = new MethodCallDependency(originMember, targetMember);
            return new object[] {originMember, expectedDependency};
        }

        private static object[] BuildMethodSignatureDependencyTestData(Type originType,
            string nameOfOriginMember, Type targetType)
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMethodMembersWithName(nameOfOriginMember).Single();
            var target = Architecture.GetTypeOfType(targetType);
            var expectedDependency = new MethodSignatureDependency(originMember, target);
            return new object[] {originMember, expectedDependency};
        }

        public class MethodCallDependencyTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodCallDependencyData = new List<object[]>
            {
                BuildMethodCallDependencyTestData(typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(), typeof(ClassWithMethodB),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()),
                BuildMethodCallDependencyTestData(typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(), typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()),
                BuildMethodCallDependencyTestData(typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(), typeof(ClassWithMethodA),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()),
                BuildMethodCallDependencyTestData(typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(), typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName())
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

        public class MethodSignatureDependencyTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodSignatureDependencyData = new List<object[]>
            {
                BuildMethodSignatureDependencyTestData(typeof(ClassWithMethodSignatureA),
                    nameof(ClassWithMethodSignatureA.MethodA).BuildMethodMemberName(typeof(ClassWithMethodSignatureB)),
                    typeof(ClassWithMethodSignatureB)),
                BuildMethodSignatureDependencyTestData(typeof(ClassWithMethodSignatureB),
                    nameof(ClassWithMethodSignatureB.MethodB).BuildMethodMemberName(typeof(ClassWithMethodSignatureA)),
                    typeof(ClassWithMethodSignatureA)),
                BuildMethodSignatureDependencyTestData(typeof(ClassWithMethodSignatureC),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName(typeof(ClassWithMethodSignatureB)),
                    typeof(ClassWithMethodSignatureB))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _methodSignatureDependencyData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class ConstructorTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodCallDependencyData = new List<object[]>
            {
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(typeof(ClassWithMethodA)),
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(typeof(ClassWithMethodA)),
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(typeof(ClassWithConstructors))
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