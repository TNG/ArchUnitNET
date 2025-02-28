using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Domain.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;

namespace ArchUnitNETTests.Domain
{
    public static class MemberTestBuild
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildOriginMemberTestData(Type originType, string memberName)
        {
            return new object[]
            {
                Architecture
                    .GetClassOfType(originType)
                    .GetMembersWithName(memberName)
                    .SingleOrDefault(),
            };
        }

        public class OriginMemberTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _originMemberData = new List<object[]>
            {
                BuildOriginMemberTestData(
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()
                ),
                BuildOriginMemberTestData(typeof(ClassWithFieldA), nameof(ClassWithFieldA.FieldA)),
                BuildOriginMemberTestData(
                    typeof(ClassWithPropertyA),
                    nameof(ClassWithPropertyA.PropertyA)
                ),
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
    }
}
