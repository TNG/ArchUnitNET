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

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent;
using JetBrains.Annotations;
using Xunit;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace ArchUnitNETTests.Domain
{
    public class MemberTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly MemberEquivalencyTestData _methodMemberEquivalencyTestData;
        private readonly MemberEquivalencyTestData _fieldMemberEquivalencyTestData;
        private readonly MemberEquivalencyTestData _propertyMemberEquivalencyTestData;

        
        public MemberTests()
        {
            _methodMemberEquivalencyTestData = new MemberEquivalencyTestData(typeof(ClassWithMethodA),
                nameof(ClassWithMethodA.MethodA).BuildMethodMemberName());
            _fieldMemberEquivalencyTestData =
                new MemberEquivalencyTestData(typeof(ClassWithFieldA), nameof(ClassWithFieldA.FieldA));
            _propertyMemberEquivalencyTestData = new MemberEquivalencyTestData(typeof(ClassWithPropertyA),
                nameof(ClassWithPropertyA.PropertyA));
        }
        
        [Fact]
        public void MethodMemberEquivalencyTests()
        {
            //Setup
            if (!(_methodMemberEquivalencyTestData.OriginMember is MethodMember memberReferenceDuplicate))
            {
                return;
            }
            object objectReferenceDuplicate = _methodMemberEquivalencyTestData.OriginMember;
            
            //Assert
            MemberHasConsistentHashCode(_methodMemberEquivalencyTestData.OriginMember,
                _methodMemberEquivalencyTestData.DuplicateMember);
            DuplicateMembersAreEqual(_methodMemberEquivalencyTestData.OriginMember,
                _methodMemberEquivalencyTestData.DuplicateMember);
            MemberDoesNotEqualNull(_methodMemberEquivalencyTestData.OriginMember);
            
            DuplicateMemberReferencesAreEqual(_methodMemberEquivalencyTestData.OriginMember, memberReferenceDuplicate);
            DuplicateMemberObjectReferencesAreEqual(_methodMemberEquivalencyTestData.OriginMember,
                objectReferenceDuplicate);
        }


        
        [Fact]
        public void FieldMemberEquivalencyTests()
        {
            //Setup
            if (!(_fieldMemberEquivalencyTestData.OriginMember is FieldMember memberReferenceDuplicate))
            {
                return;
            }
            object objectReferenceDuplicate = _fieldMemberEquivalencyTestData.OriginMember;
            
            //Assert
            MemberHasConsistentHashCode(_fieldMemberEquivalencyTestData.OriginMember,
                _fieldMemberEquivalencyTestData.DuplicateMember);
            DuplicateMembersAreEqual(_fieldMemberEquivalencyTestData.OriginMember,
                _fieldMemberEquivalencyTestData.DuplicateMember);
            MemberDoesNotEqualNull(_fieldMemberEquivalencyTestData.OriginMember);
            
            DuplicateMemberReferencesAreEqual(_fieldMemberEquivalencyTestData.OriginMember, memberReferenceDuplicate);
            DuplicateMemberObjectReferencesAreEqual(_fieldMemberEquivalencyTestData.OriginMember,
                objectReferenceDuplicate);
        }
        
        [Fact]
        public void PropertyMemberEquivalencyTests()
        {
            //Setup
            if (!(_propertyMemberEquivalencyTestData.OriginMember is PropertyMember memberReferenceDuplicate))
            {
                return;
            }
            object objectReferenceDuplicate = _propertyMemberEquivalencyTestData.OriginMember;
            
            //Assert
            MemberHasConsistentHashCode(_propertyMemberEquivalencyTestData.OriginMember,
                _propertyMemberEquivalencyTestData.DuplicateMember);
            DuplicateMembersAreEqual(_propertyMemberEquivalencyTestData.OriginMember,
                _propertyMemberEquivalencyTestData.DuplicateMember);
            MemberDoesNotEqualNull(_propertyMemberEquivalencyTestData.OriginMember);
            
            DuplicateMemberReferencesAreEqual(_propertyMemberEquivalencyTestData.OriginMember, memberReferenceDuplicate);
            DuplicateMemberObjectReferencesAreEqual(_propertyMemberEquivalencyTestData.OriginMember,
                objectReferenceDuplicate);
        }
        

        [Theory]
        [ClassData(typeof(MemberTestBuild.OriginMemberTestData))]
        public void BackwardsDependenciesTests(IMember originMember)
        {
            BackwardsDependenciesByTargetType(originMember);
            MemberMemberBackwardsDependenciesByTargetMember(originMember);
            MemberTypeBackwardsDependenciesByTargetType(originMember);
        }
        private static void DuplicateMembersAreEqual([NotNull] IMember originMember, [NotNull] object duplicateMember)
        {
            originMember.RequiredNotNull();
            duplicateMember.RequiredNotNull();

            Assert.Equal(originMember, duplicateMember);
        }

        private static void DuplicateMemberObjectReferencesAreEqual([NotNull] IMember originMember,
            object objectReferenceDuplicate)
        {
            originMember.RequiredNotNull();

            Assert.Equal(originMember, objectReferenceDuplicate);
        }

        private static void DuplicateMemberReferencesAreEqual([NotNull] IMember originMember,
            [NotNull] IMember memberReferenceDuplicate)
        {
            originMember.RequiredNotNull();
            memberReferenceDuplicate.RequiredNotNull();
            
            Assert.True(originMember.Equals(memberReferenceDuplicate));
        }

        private static void MemberDoesNotEqualNull([NotNull] IMember member)
        {
            member.RequiredNotNull();
            
            Assert.False(member.Equals(null));
        }

        private static void MemberHasConsistentHashCode([NotNull] IMember originMember,
            [NotNull] object duplicateMember)
        {
            originMember.RequiredNotNull();
            duplicateMember.RequiredNotNull();
            
            var hash = originMember.GetHashCode();
            var duplicateHash = duplicateMember.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }

        private static void BackwardsDependenciesByTargetType([NotNull] IMember originMember)
        {
            originMember.BackwardsDependencies.ForEach(backwardsDependency =>
            {
                Assert.Contains(backwardsDependency, backwardsDependency.Origin.Dependencies);
            });
        }

        private static void MemberMemberBackwardsDependenciesByTargetMember([NotNull] IMember originMember)
        {
            originMember.MemberBackwardsDependencies
                .OfType<IMemberMemberDependency>()
                .ForEach(memberMemberBackwardsDependency =>
                {
                    Assert.Contains(memberMemberBackwardsDependency,
                        memberMemberBackwardsDependency.OriginMember.MemberDependencies);
                });
        }

        private static void MemberTypeBackwardsDependenciesByTargetType([NotNull] IMember originMember)
        {
            originMember.MemberBackwardsDependencies
                .OfType<IMemberTypeDependency>()
                .ForEach(memberTypeBackwardsDependency =>
                    Assert.Contains(memberTypeBackwardsDependency,
                        memberTypeBackwardsDependency.Origin.Dependencies));
        }

        private class MemberEquivalencyTestData
        {
            public MemberEquivalencyTestData([NotNull] Type originType, [NotNull] string originMemberName)
            {
                var methodOriginClass = Architecture.GetClassOfType(originType);
                OriginMember = methodOriginClass.GetMembersWithName(originMemberName).SingleOrDefault();
                DuplicateMember = methodOriginClass.GetMembersWithName(originMemberName).SingleOrDefault();
            }

            public IMember OriginMember { get; }
            public object DuplicateMember { get; }
        }
    }
}