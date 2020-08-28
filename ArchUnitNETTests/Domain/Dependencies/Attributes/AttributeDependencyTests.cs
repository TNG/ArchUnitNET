//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Domain.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;
using TestAssembly;
using Xunit;
using static ArchUnitNETTests.Domain.Dependencies.Attributes.AttributeAssertionRepository;

namespace ArchUnitNETTests.Domain.Dependencies.Attributes
{
    public class AttributeDependencyTests
    {
        public AttributeDependencyTests()
        {
            var eventHandler = Architecture.GetInterfaceOfType(typeof(IEventHandler<>));
            _eventHandlerImplementClasses = Architecture.Classes.Where(cls => cls.ImplementsInterface(eventHandler));
            _originClass = Architecture.GetClassOfType(typeof(ClassWithInnerAttributeDependency));
            _hello = Architecture.GetClassOfType(typeof(Hello));
            _helloEvent = Architecture.GetClassOfType(typeof(HelloEvent));

            _class1 = Architecture.GetClassOfType(typeof(Class1));
            _class2 = Architecture.GetClassOfType(typeof(Class2));
            _classWithAttribute = Architecture.GetClassOfType(typeof(ClassWithExampleAttribute));
            _classWithBodyTypeA = Architecture.GetClassOfType(typeof(ClassWithBodyTypeA));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.AttributeDependencyTestArchitecture;
        private readonly IEnumerable<Class> _eventHandlerImplementClasses;
        private readonly Class _originClass;
        private readonly Class _hello;
        private readonly Class _helloEvent;
        private readonly Class _class1;
        private readonly Class _class2;
        private readonly Class _classWithAttribute;
        private readonly Class _classWithBodyTypeA;

        [Theory]
        [ClassData(typeof(AttributeTestsBuild.TypeAttributesAreFoundData))]
        public void TypeAttributesAreFound(IType targetType, Class attributeClass, Attribute attribute)
        {
            TypeAttributeAsExpected(targetType, attributeClass, attribute);
            AttributeDependencyAsExpected(targetType, attributeClass);
        }

        [Theory]
        [ClassData(typeof(AttributeTestsBuild.MemberAttributesAreFoundData))]
        public void MemberAttributesAreFound(IMember targetMember, Class attributeClass,
            Attribute attribute)
        {
            MemberAttributeAsExpected(targetMember, attributeClass, attribute);
            AttributeDependencyAsExpected(targetMember, attributeClass);
        }

        [Fact]
        public void ClassAttributeInnerDependencyAssignedToOriginClass()
        {
            //Setup
            var expectedClassTargets = new[] {_hello, _helloEvent};

            //Assert
            Assert.All(expectedClassTargets, targetClass => Assert.True(_originClass.DependsOn(targetClass.FullName)));
        }

        [Fact]
        public void ForbidAttributeForClass()
        {
            Assert.All(_eventHandlerImplementClasses, cls => Assert.False(cls.DependsOn("forbidden")));
        }

        [Fact]
        public void MemberAttributeInnerDependencyAssignedToOriginClass()
        {
            //Setup
            var expectedClassTargets = new[] {_class1, _class2, _classWithAttribute, _classWithBodyTypeA};

            //Assert
            Assert.All(expectedClassTargets, targetClass => Assert.True(_originClass.DependsOn(targetClass.FullName)));
        }

        [Fact]
        public void OriginAsExpected()
        {
            Assert.All(_originClass.GetAttributeTypeDependencies(),
                dependency => Assert.True(dependency.Origin.Equals(_originClass)));
        }
    }
}