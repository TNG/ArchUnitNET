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

using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;

namespace ArchUnitNETTests.Dependencies.Members
{
    public class ClassDependenciesIncludeMemberDependencies
    {
        [Theory]
        [ClassData(typeof(ClassDependenciesIncludeMemberDependenciesBuild.MethodDependenciesWithClassTestData))]
        public void DependenciesOfMembersAreDependenciesOfDeclaringType(Class clazz)
        {
            var methodMembers = clazz.GetMethodMembers();
            methodMembers.ForEach(methodMember =>
                Assert.True(clazz.HasDependencies(methodMember.MemberDependencies)));
        }
    }


    public static class ClassDependenciesIncludeMemberDependenciesBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

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

        internal static object[] BuildClassTestData(System.Type originType)
        {
            var originClass = Architecture.GetClassOfType(originType);
            return new object[] {originClass};
        }
    }
}