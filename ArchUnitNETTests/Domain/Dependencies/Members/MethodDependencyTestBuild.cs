using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Fluent.Extensions;
using Type = System.Type;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public static class MethodDependencyTestBuild
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildMethodCallDependencyTestData(
            Type originType,
            string nameOfOriginMember,
            Type targetType,
            string nameOfTargetMember
        )
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMembersWithName(nameOfOriginMember).Single();
            var targetClass = Architecture.GetClassOfType(targetType);
            var targetMember = targetClass.GetMethodMembersWithName(nameOfTargetMember).Single();
            var expectedDependency = new MethodCallDependency(
                originMember,
                new MethodMemberInstance(
                    targetMember,
                    Enumerable.Empty<GenericArgument>(),
                    Enumerable.Empty<GenericArgument>()
                )
            );
            return new object[] { originMember, expectedDependency };
        }

        private static object[] BuildMethodSignatureDependencyTestData(
            Type originType,
            string nameOfOriginMember,
            Type targetType
        )
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMethodMembersWithName(nameOfOriginMember).Single();
            var target = Architecture.GetClassOfType(targetType);
            var expectedDependency = new MethodSignatureDependency(
                originMember,
                new TypeInstance<Class>(target)
            );
            return new object[] { originMember, expectedDependency };
        }

        public class MethodCallDependencyTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodCallDependencyData = new List<object[]>
            {
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(),
                    typeof(ClassWithMethodB),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(),
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(),
                    typeof(ClassWithMethodA),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(),
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()
                ),
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

        public class MethodCallDependencyInAsyncMethodTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _methodCallDependencyData = new List<object[]>
            {
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodAAsync),
                    nameof(ClassWithMethodAAsync.MethodAAsync).BuildMethodMemberName(),
                    typeof(ClassWithMethodB),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodAAsync),
                    nameof(ClassWithMethodAAsync.MethodAAsync).BuildMethodMemberName(),
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(),
                    typeof(ClassWithMethodA),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName()
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName(),
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()
                ),
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
                BuildMethodSignatureDependencyTestData(
                    typeof(ClassWithMethodSignatureA),
                    nameof(ClassWithMethodSignatureA.MethodA)
                        .BuildMethodMemberName(typeof(ClassWithMethodSignatureB)),
                    typeof(ClassWithMethodSignatureB)
                ),
                BuildMethodSignatureDependencyTestData(
                    typeof(ClassWithMethodSignatureB),
                    nameof(ClassWithMethodSignatureB.MethodB)
                        .BuildMethodMemberName(typeof(ClassWithMethodSignatureA)),
                    typeof(ClassWithMethodSignatureA)
                ),
                BuildMethodSignatureDependencyTestData(
                    typeof(ClassWithMethodSignatureC),
                    StaticConstants.ConstructorNameBase.BuildMethodMemberName(
                        typeof(ClassWithMethodSignatureB)
                    ),
                    typeof(ClassWithMethodSignatureB)
                ),
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
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(
                    typeof(ClassWithMethodA)
                ),
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(
                    typeof(ClassWithMethodA)
                ),
                ClassDependenciesIncludeMemberDependenciesBuild.BuildClassTestData(
                    typeof(ClassWithConstructors)
                ),
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
