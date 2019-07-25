/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

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