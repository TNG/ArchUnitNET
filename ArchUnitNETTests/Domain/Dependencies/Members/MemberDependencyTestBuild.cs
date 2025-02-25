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
    public static class MemberDependencyTestBuild
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private static object[] BuildBodyTypeMemberDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType
        )
        {
            var classMemberInfo = BuildMemberDependencyTestData(
                originType,
                originMemberName,
                targetType
            );

            if (!(classMemberInfo.OriginMember is MethodMember originMember))
            {
                return new object[] { null, null, null, null };
            }

            var memberTypeDependency = new BodyTypeMemberDependency(
                originMember,
                new TypeInstance<IType>(classMemberInfo.TargetClass)
            );
            object duplicateMemberTypeDependency = new BodyTypeMemberDependency(
                originMember,
                new TypeInstance<IType>(classMemberInfo.TargetClass)
            );
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;

            return new[]
            {
                memberTypeDependency,
                duplicateMemberTypeDependency,
                dependencyReferenceDuplicate,
                objectReferenceDuplicate,
            };
        }

        private static object[] BuildFieldDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType
        )
        {
            var classMemberInfo = BuildMemberDependencyTestData(
                originType,
                originMemberName,
                targetType
            );

            if (!(classMemberInfo.OriginMember is FieldMember originMember))
            {
                return new object[] { null, null, null, null };
            }

            var memberTypeDependency = new FieldTypeDependency(originMember);
            object duplicateMemberTypeDependency = new FieldTypeDependency(originMember);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;

            return new[]
            {
                memberTypeDependency,
                duplicateMemberTypeDependency,
                dependencyReferenceDuplicate,
                objectReferenceDuplicate,
            };
        }

        private static object[] BuildMethodCallDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType,
            string targetMemberName
        )
        {
            var classMemberInfo = BuildMemberDependencyTestData(
                originType,
                originMemberName,
                targetType,
                targetMemberName
            );

            if (!(classMemberInfo.TargetMember is MethodMember targetMember))
            {
                return new object[] { null, null, null, null };
            }

            var memberTypeDependency = new MethodCallDependency(
                classMemberInfo.OriginMember,
                new MethodMemberInstance(
                    targetMember,
                    Enumerable.Empty<GenericArgument>(),
                    Enumerable.Empty<GenericArgument>()
                )
            );
            object duplicateMemberTypeDependency = new MethodCallDependency(
                classMemberInfo.OriginMember,
                new MethodMemberInstance(
                    targetMember,
                    Enumerable.Empty<GenericArgument>(),
                    Enumerable.Empty<GenericArgument>()
                )
            );
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;

            return new[]
            {
                memberTypeDependency,
                duplicateMemberTypeDependency,
                dependencyReferenceDuplicate,
                objectReferenceDuplicate,
            };
        }

        private static object[] BuildMethodSignatureDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType
        )
        {
            var classMemberInfo = BuildMemberDependencyTestData(
                originType,
                originMemberName,
                targetType
            );
            if (!(classMemberInfo.OriginMember is MethodMember originMember))
            {
                return new object[] { null, null, null, null };
            }

            var memberTypeDependency = new MethodSignatureDependency(
                originMember,
                new TypeInstance<IType>(classMemberInfo.TargetClass)
            );
            object duplicateMemberTypeDependency = new MethodSignatureDependency(
                originMember,
                new TypeInstance<IType>(classMemberInfo.TargetClass)
            );
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;

            return new[]
            {
                memberTypeDependency,
                duplicateMemberTypeDependency,
                dependencyReferenceDuplicate,
                objectReferenceDuplicate,
            };
        }

        private static object[] BuildPropertyDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType
        )
        {
            var classMemberInfo = BuildMemberDependencyTestData(
                originType,
                originMemberName,
                targetType
            );
            if (!(classMemberInfo.OriginMember is PropertyMember originMember))
            {
                return new object[] { null, null, null, null };
            }

            var memberTypeDependency = new PropertyTypeDependency(originMember);
            object duplicateMemberTypeDependency = new PropertyTypeDependency(originMember);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;

            return new[]
            {
                memberTypeDependency,
                duplicateMemberTypeDependency,
                dependencyReferenceDuplicate,
                objectReferenceDuplicate,
            };
        }

        private static ClassMemberInfo BuildMemberDependencyTestData(
            Type originType,
            string originMemberName,
            Type targetType,
            string targetMemberName = null
        )
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMembersWithName(originMemberName).SingleOrDefault();
            var targetClass = Architecture.GetClassOfType(targetType);
            var targetMember = targetClass.GetMembersWithName(targetMemberName).SingleOrDefault();

            return new ClassMemberInfo(originClass, originMember, targetClass, targetMember);
        }

        public class MemberDependencyModelingTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _memberDependencyModelingData = new List<object[]>
            {
                BuildPropertyDependencyTestData(
                    typeof(ClassWithPropertyA),
                    nameof(ClassWithPropertyA.PropertyA),
                    typeof(PropertyType)
                ),
                BuildBodyTypeMemberDependencyTestData(
                    typeof(ClassWithBodyTypeA),
                    nameof(ClassWithBodyTypeA.MethodWithTypeA).BuildMethodMemberName(),
                    typeof(TypeA)
                ),
                BuildFieldDependencyTestData(
                    typeof(ClassWithFieldA),
                    nameof(ClassWithFieldA.FieldA),
                    typeof(FieldType)
                ),
                BuildMethodCallDependencyTestData(
                    typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(),
                    typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()
                ),
                BuildMethodSignatureDependencyTestData(
                    typeof(ClassWithMethodSignatureA),
                    nameof(ClassWithMethodSignatureA.MethodA).BuildMethodMemberName(),
                    typeof(ClassWithMethodSignatureB)
                ),
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _memberDependencyModelingData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    public class ClassMemberInfo
    {
        public ClassMemberInfo(
            Class originClass,
            IMember originMember,
            Class targetClass,
            IMember targetMember
        )
        {
            OriginClass = originClass;
            OriginMember = originMember;
            TargetClass = targetClass;
            TargetMember = targetMember;
        }

        public Class OriginClass { get; }
        public IMember OriginMember { get; }
        public Class TargetClass { get; }
        public IMember TargetMember { get; }
    }
}
