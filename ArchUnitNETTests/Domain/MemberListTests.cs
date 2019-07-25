/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class MemberListTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly MemberList _memberList;
        private readonly FieldMember _fieldA;
        private readonly PropertyMember _propertyA;
        private readonly MethodMember _methodA;
        private readonly List<IMember> _listOfMembers;

        public MemberListTests()
        {
            _fieldA = Architecture.GetClassOfType(typeof(ClassWithFieldA))
                .GetFieldMembersWithName(nameof(ClassWithFieldA.FieldA)).SingleOrDefault();
            _propertyA = Architecture.GetClassOfType(typeof(ClassWithPropertyA))
                .GetPropertyMembersWithName(nameof(ClassWithPropertyA.PropertyA)).SingleOrDefault();
            _methodA = Architecture.GetClassOfType(typeof(ClassWithMethodA))
                .GetMethodMembersWithName(nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()).SingleOrDefault();
            
            _memberList = new MemberList();
            _listOfMembers = new List<IMember>{_fieldA, _propertyA, _methodA};
        }

        [Fact]
        public void NewListsAreEqual()
        {
            var newMemberList = new MemberList();
            Assert.True(_memberList.Equals(newMemberList));
        }

        [Fact]
        public void ListOfMembersAsExpected()
        {
            Assert.Equal(_listOfMembers[0], _fieldA);
            Assert.Equal(_listOfMembers[1], _propertyA);
            Assert.Equal(_listOfMembers[2], _methodA);
        }
        
        [Fact]
        public void MemberListFromExistingListAsExpected()
        {
            var memberListFromExistingList = new MemberList(_listOfMembers);
            _listOfMembers.ForEach(member => Assert.Contains(member, memberListFromExistingList));
        }

        [Fact]
        public void AddListMemberAsExpected()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_fieldA);
            Assert.Single(_memberList);
            Assert.Contains(_fieldA, _memberList);
        }

        [Fact]
        public void ClearListAsExpected()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_fieldA);
            Assert.Single(_memberList);
            _memberList.Clear();
            Assert.Empty(_memberList);
        }

        [Fact]
        public void SuccessfullyRemoveMemberListMember()
        {
            _memberList.Add(_methodA);
            _memberList.Add(_propertyA);
            Assert.Contains(_propertyA, _memberList);
            _memberList.Remove(_propertyA);
            Assert.DoesNotContain(_propertyA, _memberList);
            Assert.Single(_memberList);
        }

        [Fact]
        public void SuccessfullyGetMemberIndex()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_methodA);
            Assert.Single(_memberList);
            Assert.Equal(0, _memberList.IndexOf(_methodA));
        }

        [Fact]
        public void MemberListFromExistingListMatchesIndices()
        {
            var memberListFromExistingList = new MemberList(_listOfMembers);
            Assert.Equal(0, memberListFromExistingList.IndexOf(_fieldA));
            Assert.Equal(1, memberListFromExistingList.IndexOf(_propertyA));
            Assert.Equal(2, memberListFromExistingList.IndexOf(_methodA));
        }

        [Fact]
        public void SuccessfullyRemoveAtIndex()
        {
            var memberListFromExistingList = new MemberList(_listOfMembers);
            Assert.Equal(_propertyA, memberListFromExistingList[1]);
            Assert.Equal(3, memberListFromExistingList.Count);
            memberListFromExistingList.RemoveAt(1);
            Assert.Equal(_methodA, memberListFromExistingList[1]);
        }


        [Fact]
        public void SuccessfullySetAtIndex()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_methodA);
            _memberList.Add(_fieldA);
            Assert.Equal(_fieldA, _memberList[1]);
            _memberList[1] = _propertyA;
            Assert.Equal(_propertyA, _memberList[1]);
        }

        [Fact]
        public void SuccessfullyGetMemberByName()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_methodA);
            var memberFromMethodAName = _memberList[nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()];
            Assert.Equal(_methodA, memberFromMethodAName);
        }

        [Fact]
        public void SuccessfullySetMemberByName()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_methodA);
            _memberList[nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()] = _fieldA;
            Assert.Single(_memberList);
            Assert.Contains(_fieldA, _memberList);
        }

        [Fact]
        public void SuccessfullyInsertAtIndex()
        {
            Assert.Empty(_memberList);
            _memberList.Add(_methodA);
            Assert.Equal(_methodA, _memberList[0]);
            _memberList.Insert(0, _fieldA);
            Assert.Equal(_fieldA, _memberList[0]);
            Assert.Equal(_methodA, _memberList[1]);
        }
    }
}