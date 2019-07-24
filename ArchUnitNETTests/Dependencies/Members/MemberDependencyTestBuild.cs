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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;

namespace ArchUnitNETTests.Dependencies.Members
{
    public static class MemberDependencyTestBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;
        public class MemberDependencyModelingTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _memberDependencyModelingData = new List<object[]>
            {
                BuildPropertyDependencyTestData(typeof(ClassWithPropertyA), nameof(ClassWithPropertyA.PropertyA),
                    typeof(PropertyType)),
                BuildBodyTypeMemberDependencyTestData(typeof(ClassWithBodyTypeA),
                    nameof(ClassWithBodyTypeA.MethodWithTypeA).BuildMethodMemberName(), typeof(TypeA)),
                BuildFieldDependencyTestData(typeof(ClassWithFieldA), nameof(ClassWithFieldA.FieldA),
                    typeof(FieldType)),
                BuildMethodCallDependencyTestData(typeof(ClassWithMethodA),
                    nameof(ClassWithMethodA.MethodA).BuildMethodMemberName(), typeof(ClassWithMethodB),
                    nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()),
                BuildMethodSignatureDependencyTestData(typeof(ClassWithMethodSignatureA),
                    nameof(ClassWithMethodSignatureA.MethodA).BuildMethodMemberName(),
                    typeof(ClassWithMethodSignatureB))
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

        private static object[] BuildBodyTypeMemberDependencyTestData(Type originType, string originMemberName,
            Type targetType)
        {
            var classMemberInfo =
                BuildMemberDependencyTestData(originType, originMemberName, targetType);

            if (!(classMemberInfo.OriginMember is MethodMember originMember))
            {
                return new object[] {null, null, null, null};
            }

            var memberTypeDependency = new BodyTypeMemberDependency(originMember, classMemberInfo.TargetClass);
            object duplicateMemberTypeDependency =
                new BodyTypeMemberDependency(originMember, classMemberInfo.TargetClass);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;
            
            return new[]
            {
                memberTypeDependency, duplicateMemberTypeDependency, dependencyReferenceDuplicate,
                objectReferenceDuplicate
            };
        }

        private static object[] BuildFieldDependencyTestData(Type originType, string originMemberName, Type targetType)
        {
            var classMemberInfo = BuildMemberDependencyTestData(originType, originMemberName, targetType);
            
            if (!(classMemberInfo.OriginMember is FieldMember originMember))
            {
                return new object[] {null, null, null, null};
            }
            
            var memberTypeDependency = new FieldTypeDependency(originMember);
            object duplicateMemberTypeDependency = new FieldTypeDependency(originMember);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;
            
            return new[]
            {
                memberTypeDependency, duplicateMemberTypeDependency, dependencyReferenceDuplicate,
                objectReferenceDuplicate
            };
        }

        private static object[] BuildMethodCallDependencyTestData(Type originType, string originMemberName,
            Type targetType, string targetMemberName)
        {
            var classMemberInfo = BuildMemberDependencyTestData(originType, originMemberName, targetType,
                targetMemberName);

            if (!(classMemberInfo.TargetMember is MethodMember targetMember))
            {
                return new object[] {null, null, null, null};
            }
            
            var memberTypeDependency = new MethodCallDependency(classMemberInfo.OriginMember, targetMember);
            object duplicateMemberTypeDependency = new MethodCallDependency(classMemberInfo.OriginMember, targetMember);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;
            
            return new[]
            {
                memberTypeDependency, duplicateMemberTypeDependency, dependencyReferenceDuplicate,
                objectReferenceDuplicate
            };
        }

        private static object[] BuildMethodSignatureDependencyTestData(Type originType, string originMemberName,
            Type targetType)
        {
            var classMemberInfo = BuildMemberDependencyTestData(originType, originMemberName, targetType);
            if (!(classMemberInfo.OriginMember is MethodMember originMember))
            {
                return new object[] {null, null, null, null};
            }
            
            var memberTypeDependency = new MethodSignatureDependency(originMember, classMemberInfo.TargetClass);
            object duplicateMemberTypeDependency =
                new MethodSignatureDependency(originMember, classMemberInfo.TargetClass);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;
            
            return new[]
            {
                memberTypeDependency, duplicateMemberTypeDependency, dependencyReferenceDuplicate,
                objectReferenceDuplicate
            };
        }

        private static object[] BuildPropertyDependencyTestData(Type originType, string originMemberName,
            Type targetType)
        {
            var classMemberInfo = BuildMemberDependencyTestData(originType, originMemberName, targetType);
            if (!(classMemberInfo.OriginMember is PropertyMember originMember))
            {
                return new object[] {null, null, null, null};
            }
            
            var memberTypeDependency = new PropertyTypeDependency(originMember);
            object duplicateMemberTypeDependency = new PropertyTypeDependency(originMember);
            var dependencyReferenceDuplicate = memberTypeDependency;
            object objectReferenceDuplicate = memberTypeDependency;
            
            return new[]
            {
                memberTypeDependency, duplicateMemberTypeDependency, dependencyReferenceDuplicate,
                objectReferenceDuplicate
            };
        }
        
        private static ClassMemberInfo BuildMemberDependencyTestData(Type originType, string originMemberName,
            Type targetType, string targetMemberName = null)
        {
            var originClass = Architecture.GetClassOfType(originType);
            var originMember = originClass.GetMembersWithName(originMemberName).SingleOrDefault();
            var targetClass = Architecture.GetClassOfType(targetType);
            var targetMember = targetClass.GetMembersWithName(targetMemberName).SingleOrDefault();

            return new ClassMemberInfo(originClass, originMember, targetClass, targetMember);
        }
    }

    public class ClassMemberInfo
    {
        public ClassMemberInfo(Class originClass, IMember originMember, Class targetClass, IMember targetMember)
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