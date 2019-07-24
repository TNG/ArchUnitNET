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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent;

namespace ArchUnitNETTests.Domain
{
    public static class MemberTestBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;

        public class OriginMemberTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _originMemberData = new List<object[]>
            {
                BuildOriginMemberTestData(typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()),
                BuildOriginMemberTestData(typeof(ClassWithFieldA), nameof(ClassWithFieldA.FieldA)),
                BuildOriginMemberTestData(typeof(ClassWithPropertyA), nameof(ClassWithPropertyA.PropertyA))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _originMemberData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static object[] BuildOriginMemberTestData(System.Type originType, string memberName)
        {
            return new object[] {Architecture.GetClassOfType(originType).GetMembersWithName(memberName)
                .SingleOrDefault()};
        }
    }
}