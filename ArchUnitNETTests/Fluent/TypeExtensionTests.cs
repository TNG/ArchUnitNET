/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using Xunit;

namespace ArchUnitNETTests.Fluent
{
    public class TypeExtensionTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Class _methodOriginClass;
        private readonly IMember _methodMember;
        private readonly Class _fieldOriginClass;
        private readonly IMember _fieldMember;
        private readonly Class _propertyOriginClass;
        private readonly IMember _propertyMember;

        private readonly Class _exampleAttribute;

        private const string ExpectedAttributeNamespace =
            StaticConstants.ArchUnitNETTestsDependenciesAttributesNamespace;

        private readonly Class _regexUtilsTests;
        private const string ExpectedRegexUtilsTestNamespace = StaticConstants.ArchUnitNETTestsFluentNamespace;

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

        [Fact]
        public void MethodMemberFoundFromMembers()
        {
            Assert.True(_methodMember is MethodMember);
            Assert.NotNull(_methodOriginClass.Members[_methodMember.Name]);
        }

        [Fact]
        public void FieldMemberFoundFromMembers()
        {
            Assert.True(_fieldMember is FieldMember);
            Assert.NotNull(_fieldOriginClass.Members[_fieldMember.Name]);
        }
        
        [Fact]
        public void PropertyMemberFoundFromMembers()
        {
            Assert.True(_propertyMember is PropertyMember);
            Assert.NotNull(_propertyOriginClass.Members[_propertyMember.Name]);
        }

        [Fact]
        public void NamespaceMatchAsExpected()
        {
            Assert.True(_exampleAttribute.ResidesInNamespace(ExpectedAttributeNamespace));
            Assert.True(_regexUtilsTests.ResidesInNamespace(ExpectedRegexUtilsTestNamespace));
            Assert.True(_exampleAttribute.ResidesInNamespace(string.Empty));
        }
    }
}