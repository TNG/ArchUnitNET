/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace ArchUnitNETTests.Domain
{
    public class MemberTests
    {
        public MemberTests()
        {
            _methodMemberEquivalencyTestData = new MemberEquivalencyTestData(typeof(ClassWithMethodA),
                nameof(ClassWithMethodA.MethodA).BuildMethodMemberName());
            _fieldMemberEquivalencyTestData =
                new MemberEquivalencyTestData(typeof(ClassWithFieldA), nameof(ClassWithFieldA.FieldA));
            _propertyMemberEquivalencyTestData = new MemberEquivalencyTestData(typeof(ClassWithPropertyA),
                nameof(ClassWithPropertyA.PropertyA));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly MemberEquivalencyTestData _methodMemberEquivalencyTestData;
        private readonly MemberEquivalencyTestData _fieldMemberEquivalencyTestData;
        private readonly MemberEquivalencyTestData _propertyMemberEquivalencyTestData;


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

        [Fact]
        public void CorrectlyAssignNotAccessibleGetter()
        {
            Assert.Contains(Architecture.PropertyMembers.WhereNameIs("FieldWithoutGetter"),
                member => member.Visibility == NotAccessible);
        }

        [Fact]
        public void CorrectlyAssignNotAccessibleSetter()
        {
            Assert.Contains(Architecture.PropertyMembers.WhereNameIs("FieldWithoutSetter"),
                member => member.SetterVisibility == NotAccessible);
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
        public void FieldMembersMustHaveAccessibleVisibility()
        {
            Assert.True(Architecture.FieldMembers.All(member => member.Visibility != NotAccessible));
        }

        [Fact]
        public void MembersAreMethodMembersAndFieldMembersAndPropertyMembers()
        {
            var members = Architecture.Members;
            var methodMembers = Architecture.MethodMembers;
            var fieldMembers = Architecture.FieldMembers;
            var propertyMembers = Architecture.PropertyMembers;
            Assert.True(members.All(member =>
                methodMembers.Contains(member) ^ fieldMembers.Contains(member) ^ propertyMembers.Contains(member)));
            Assert.True(methodMembers.All(member => members.Contains(member)));
            Assert.True(fieldMembers.All(member => members.Contains(member)));
            Assert.True(propertyMembers.All(member => members.Contains(member)));
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
        public void MethodMembersMustHaveAccessibleVisibility()
        {
            Assert.True(Architecture.MethodMembers.All(member => member.Visibility != NotAccessible));
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

            DuplicateMemberReferencesAreEqual(_propertyMemberEquivalencyTestData.OriginMember,
                memberReferenceDuplicate);
            DuplicateMemberObjectReferencesAreEqual(_propertyMemberEquivalencyTestData.OriginMember,
                objectReferenceDuplicate);
        }

        // ReSharper disable All
        private class ClassWithMemberWithNotAccessibleGetterSetter
        {
            private FieldType _fieldWithoutGetter;
            private FieldType FieldWithoutSetter { get; }

            private FieldType FieldWithoutGetter
            {
                set => _fieldWithoutGetter = value;
            }
        }

        public class FieldType
        {
        }
    }
}