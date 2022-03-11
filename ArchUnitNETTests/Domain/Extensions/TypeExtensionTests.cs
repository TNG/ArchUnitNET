//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Domain.Dependencies.Attributes;
using ArchUnitNETTests.Domain.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;
using ArchUnitNETTests.Loader;
using Xunit;

namespace ArchUnitNETTests.Domain.Extensions
{
    public class TypeExtensionTests
    {
        public TypeExtensionTests()
        {
            _methodOriginClass = Architecture.GetClassOfType(typeof(ClassWithMethodA));
            _methodMember = _methodOriginClass
                .GetMembersWithName(nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()).SingleOrDefault();
            _fieldOriginClass = Architecture.GetClassOfType(typeof(ClassWithFieldA));
            _fieldMember = _fieldOriginClass.GetMembersWithName(nameof(ClassWithFieldA.FieldA)).SingleOrDefault();
            _propertyOriginClass = Architecture.GetClassOfType(typeof(ClassWithPropertyA));
            _propertyMember = _propertyOriginClass.GetMembersWithName(nameof(ClassWithPropertyA.PropertyA))
                .SingleOrDefault();

            _exampleAttribute = Architecture.GetClassOfType(typeof(ExampleAttribute));
            _regexUtilsTests = Architecture.GetClassOfType(typeof(RegexUtilsTest));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Class _methodOriginClass;
        private readonly IMember _methodMember;
        private readonly Class _fieldOriginClass;
        private readonly IMember _fieldMember;
        private readonly Class _propertyOriginClass;
        private readonly IMember _propertyMember;

        private readonly Class _exampleAttribute;
        private const string ExpectedAttributeNamespace = "ArchUnitNETTests.Domain.Dependencies.Attributes";
        private const string ParentAttributeNamespace = "ArchUnitNETTests.Domain.Dependencies";

        private readonly Class _regexUtilsTests;
        private const string ExpectedRegexUtilsTestNamespace = "ArchUnitNETTests.Loader";

        [Fact]
        public void FieldMemberFoundFromMembers()
        {
            Assert.True(_fieldMember is FieldMember);
            Assert.NotNull(_fieldOriginClass.Members[_fieldMember.Name]);
        }

        [Fact]
        public void FullNameContainsTest()
        {
            Assert.True(_fieldMember.FullNameContains("ieLda"));
            Assert.True(_propertyOriginClass.FullNameContains("TeST"));
            Assert.False(_methodOriginClass.FullNameContains("ClassMethod"));
            Assert.True(_methodMember.FullNameContains(""));
            Assert.False(_exampleAttribute.FullNameContains(null));
        }

        [Fact]
        public void FullNameMatchesTest()
        {
            Assert.True(_fieldMember.FullNameMatches("(?i)ieLda", true));
            Assert.True(_propertyOriginClass.FullNameMatches("(?i)sswITH", true));
            Assert.False(_methodOriginClass.FullNameMatches("ClassMethod"));
            Assert.False(_methodMember.FullNameMatches(""));
            Assert.True(_methodMember.FullNameMatches(_methodMember.FullName));
            Assert.True(_methodMember.FullNameMatches(_methodMember.FullName.ToLower()));
            Assert.False(_exampleAttribute.FullNameMatches(null));
        }

        [Fact]
        public void HasMembersWithFullNameTest()
        {
            Assert.True(Architecture.Types.All(type =>
                type.GetPropertyMembers().All(member => type.HasPropertyMemberWithFullName(member.FullName))));
            Assert.True(Architecture.Types.All(type =>
                type.GetFieldMembers().All(member => type.HasFieldMemberWithFullName(member.FullName))));
            Assert.True(Architecture.Types.All(type =>
                type.GetMethodMembers().All(member => type.HasMethodMemberWithFullName(member.FullName))));
            Assert.True(Architecture.Types.All(type =>
                type.Members.All(member => type.HasMemberWithFullName(member.FullName))));
            Assert.False(_methodOriginClass.HasPropertyMemberWithFullName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasFieldMemberWithFullName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasMethodMemberWithFullName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasMemberWithFullName("nonExistentMember"));
        }

        [Fact]
        public void HasMembersWithNameTest()
        {
            Assert.True(Architecture.Types.All(type =>
                type.GetPropertyMembers().All(member => type.HasPropertyMemberWithName(member.Name))));
            Assert.True(Architecture.Types.All(type =>
                type.GetFieldMembers().All(member => type.HasFieldMemberWithName(member.Name))));
            Assert.True(Architecture.Types.All(type =>
                type.GetMethodMembers().All(member => type.HasMethodMemberWithName(member.Name))));
            Assert.True(Architecture.Types.All(type =>
                type.Members.All(member => type.HasMemberWithName(member.Name))));
            Assert.False(_methodOriginClass.HasPropertyMemberWithName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasFieldMemberWithName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasMethodMemberWithName("nonExistentMember"));
            Assert.False(_methodOriginClass.HasMemberWithName("nonExistentMember"));
        }

        [Fact]
        public void ImplementsInterfaceTest()
        {
            Assert.True(Architecture.Types.All(type =>
                type.ImplementedInterfaces.All(intf =>
                    type.ImplementsInterface(intf.FullName))));
        }

        [Fact]
        public void MethodMemberFoundFromMembers()
        {
            Assert.True(_methodMember is MethodMember);
            Assert.NotNull(_methodOriginClass.Members[_methodMember.Name]);
        }

        [Fact]
        public void NameContainsTest()
        {
            Assert.True(_fieldMember.NameContains("ieLda", StringComparison.InvariantCultureIgnoreCase));
            Assert.True(_propertyOriginClass.NameContains("sswITH", StringComparison.InvariantCultureIgnoreCase));
            Assert.False(_methodOriginClass.NameContains("ClassMethod"));
            Assert.True(_methodMember.NameContains(""));
            Assert.ThrowsAny<ArgumentNullException>(() => _exampleAttribute.NameContains(null));
        }

        [Fact]
        public void NameEndsWithTest()
        {
            Assert.True(_fieldMember.NameEndsWith("ieLda", StringComparison.InvariantCultureIgnoreCase));
            Assert.False(_propertyOriginClass.NameEndsWith("sswITH", StringComparison.InvariantCultureIgnoreCase));
            Assert.False(_methodOriginClass.NameEndsWith("ClassMethod"));
            Assert.True(_methodMember.NameEndsWith(""));
            Assert.Throws<ArgumentNullException>(() => _exampleAttribute.NameEndsWith(null));
        }

        [Fact]
        public void NameMatchesTest()
        {
            Assert.True(_fieldMember.NameMatches("(?i)ieLda", true));
            Assert.True(_propertyOriginClass.NameMatches("(?i)sswITH", true));
            Assert.False(_methodOriginClass.NameMatches("ClassMethod"));
            Assert.False(_methodMember.NameMatches(""));
            Assert.True(_methodMember.NameMatches(_methodMember.Name));
            Assert.True(_methodMember.NameMatches(_methodMember.Name.ToLower()));
            Assert.False(_exampleAttribute.NameMatches(null));
        }

        [Fact]
        public void NamespaceMatchAsExpected()
        {
            Assert.True(_exampleAttribute.ResidesInNamespace(ExpectedAttributeNamespace));
            Assert.False(_exampleAttribute.ResidesInNamespace(ParentAttributeNamespace));
            Assert.True(_exampleAttribute.ResidesInNamespace(ParentAttributeNamespace, true));
            Assert.True(_regexUtilsTests.ResidesInNamespace(ExpectedRegexUtilsTestNamespace));
            Assert.False(_exampleAttribute.ResidesInNamespace(string.Empty));
        }

        [Fact]
        public void NameStartsWithTest()
        {
            Assert.True(_fieldMember.NameStartsWith("fIEldA", StringComparison.InvariantCultureIgnoreCase));
            Assert.True(_propertyOriginClass.NameStartsWith("cLassw", StringComparison.InvariantCultureIgnoreCase));
            Assert.False(_methodOriginClass.NameStartsWith("With"));
            Assert.True(_methodMember.NameStartsWith(""));
            Assert.Throws<ArgumentNullException>(() => _exampleAttribute.NameStartsWith(null));
        }

        [Fact]
        public void PropertyMemberFoundFromMembers()
        {
            Assert.True(_propertyMember is PropertyMember);
            Assert.NotNull(_propertyOriginClass.Members[_propertyMember.Name]);
        }
    }
}