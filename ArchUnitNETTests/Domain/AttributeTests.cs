//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class AttributeTests
    {
        public AttributeTests()
        {
            _developerAttributePair = new AttributeOriginClassPair(typeof(CountryAttributeWithParameters));
            _abstractAttributePair = new AttributeOriginClassPair(typeof(ExampleAbstractAttribute));
            _attributeEquivalencyTestData = new AttributeEquivalencyTestData(typeof(CountryAttributeWithParameters));
            _iAttribute = Architecture.GetInterfaceOfType(typeof(IAttribute));
            _interfaceImplementingAttributePair = new AttributeOriginClassPair(typeof(InterfaceImplementingAttribute));
            _implementsAbstractAttribute = new Attribute(Architecture.GetClassOfType(typeof(ChildOfAbstractAttribute)));
            _unrelatedType = new Type(_abstractAttributePair.OriginClass.Name,
                _abstractAttributePair.OriginClass.FullName, _abstractAttributePair.OriginClass.Assembly,
                _abstractAttributePair.OriginClass.Namespace, _abstractAttributePair.OriginClass.Visibility,
                _abstractAttributePair.OriginClass.IsNested);
            _propertyMember = _implementsAbstractAttribute
                .GetPropertyMembersWithName(nameof(ChildOfAbstractAttribute.Property)).SingleOrDefault();
            _inheritedFieldMember = _abstractAttributePair.Attribute
                .GetFieldMembersWithName("_fieldType").SingleOrDefault();
            _constructorAttributePair = new AttributeOriginClassPair(typeof(ConstructorAttribute));
            _attributeWithAttributesPair = new AttributeOriginClassPair(typeof(AttributeWithAttributes));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly AttributeOriginClassPair _developerAttributePair;
        private readonly AttributeEquivalencyTestData _attributeEquivalencyTestData;

        private readonly AttributeOriginClassPair _abstractAttributePair;

        private readonly Attribute _implementsAbstractAttribute;
        private readonly Interface _iAttribute;

        private readonly AttributeOriginClassPair _interfaceImplementingAttributePair;

        private readonly Type _unrelatedType;
        private readonly PropertyMember _propertyMember;
        private readonly FieldMember _inheritedFieldMember;

        private readonly AttributeOriginClassPair _constructorAttributePair;

        private readonly AttributeOriginClassPair _attributeWithAttributesPair;

        private class AttributeOriginClassPair
        {
            public AttributeOriginClassPair(System.Type originType)
            {
                OriginClass = Architecture.GetClassOfType(originType);
                Attribute = Architecture.GetAttributeOfType(originType);
            }

            [NotNull] public Class OriginClass { get; }
            [NotNull] public Attribute Attribute { get; }
        }

        private class AttributeEquivalencyTestData
        {
            public AttributeEquivalencyTestData([NotNull] System.Type originType)
            {
                OriginAttribute = new Attribute(Architecture.GetClassOfType(originType));
                DuplicateAttribute =
                    new Attribute(Architecture.GetClassOfType(originType));
                AttributeReferenceDuplicate = OriginAttribute;
                ObjectReferenceDuplicate = OriginAttribute;
            }

            [NotNull] public Attribute OriginAttribute { get; }
            [NotNull] public object DuplicateAttribute { get; }
            [NotNull] public Attribute AttributeReferenceDuplicate { get; }
            [NotNull] public object ObjectReferenceDuplicate { get; }
        }

        [Fact]
        public void AssemblyAsExpected()
        {
            Assert.Equal(_developerAttributePair.OriginClass.Assembly,
                _developerAttributePair.Attribute.Assembly);
        }

        [Fact]
        public void AttributeDoesNotEqualNull()
        {
            Assert.False(_attributeEquivalencyTestData.OriginAttribute.Equals(null));
        }

        [Fact]
        public void AttributeHasConsistentHashCode()
        {
            var hash = _attributeEquivalencyTestData.OriginAttribute.GetHashCode();
            var duplicateHash = _attributeEquivalencyTestData.DuplicateAttribute.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }

        [Fact]
        public void AttributesAreSubsetOfClasses()
        {
            Assert.NotEmpty(Architecture.Attributes);
            Architecture.Attributes.ForEach(attribute => Assert.Contains(attribute, Architecture.Classes));
        }

        [Fact]
        public void AttributesCorrectlyAssigned()
        {
            Assert.Contains(StaticTestTypes.TestAttribute, Architecture.Attributes);
            Assert.Contains(StaticTestTypes.ChildTestAttribute, Architecture.Attributes);
            Assert.Contains(StaticTestTypes.SealedTestAttribute, Architecture.Attributes);
            Assert.DoesNotContain(StaticTestTypes.PublicTestClass, Architecture.Attributes);
            Assert.DoesNotContain(StaticTestTypes.InheritingType, Architecture.Attributes);
        }

        [Fact]
        public void AttributesOfAttributeAsExpected()
        {
            _attributeWithAttributesPair.OriginClass.Attributes.ForEach(originClassAttribute =>
                Assert.Contains(originClassAttribute, _attributeWithAttributesPair.Attribute.Attributes));
        }

        [Fact]
        public void ConstructorsAsExpected()
        {
            _constructorAttributePair.OriginClass.Constructors.ForEach(originClassConstructor =>
                Assert.Contains(originClassConstructor, _constructorAttributePair.Attribute.Constructors));
        }

        [Fact]
        public void DependenciesAsExpected()
        {
            Assert.Equal(_developerAttributePair.OriginClass.Dependencies,
                _developerAttributePair.Attribute.Dependencies);
        }

        [Fact]
        public void DuplicateAttributeObjectReferencesAreEqual()
        {
            Assert.True(_attributeEquivalencyTestData.OriginAttribute
                .Equals(_attributeEquivalencyTestData.ObjectReferenceDuplicate));
        }

        [Fact]
        public void DuplicateAttributeReferencesAreEqual()
        {
            Assert.True(_attributeEquivalencyTestData.OriginAttribute
                .Equals(_attributeEquivalencyTestData.AttributeReferenceDuplicate));
        }

        [Fact]
        public void DuplicateAttributesAreEqual()
        {
            Assert.True(_attributeEquivalencyTestData.OriginAttribute
                .Equals(_attributeEquivalencyTestData.DuplicateAttribute));
        }

        [Fact]
        public void ImplementedInterfacesRecognized()
        {
            _interfaceImplementingAttributePair.OriginClass.ImplementedInterfaces
                .ForEach(implementedInterface =>
                {
                    Assert.Contains(implementedInterface,
                        _interfaceImplementingAttributePair.Attribute.ImplementedInterfaces);
                });
        }

        [Fact]
        public void InheritsBaseClassDependenciesAsExpected()
        {
            var expectedInheritedDependency = new FieldTypeDependency(_inheritedFieldMember);
            Assert.Contains(expectedInheritedDependency, _implementsAbstractAttribute.DependenciesIncludingInherited);
        }

        [Fact]
        public void InheritsTypeDependenciesAsExpected()
        {
            var expectedDependency = new PropertyTypeDependency(_propertyMember);
            Assert.True(_implementsAbstractAttribute.HasDependency(expectedDependency));
        }

        [Fact]
        public void IsAssignableToImplementedInterface()
        {
            Assert.True(_interfaceImplementingAttributePair.Attribute.IsAssignableTo(_iAttribute));
        }

        [Fact]
        public void IsAssignableToItself()
        {
            Assert.True(_abstractAttributePair.Attribute.IsAssignableTo(_abstractAttributePair.Attribute));
        }

        [Fact]
        public void IsAssignableToParentAttribute()
        {
            Assert.True(_implementsAbstractAttribute.IsAssignableTo(_abstractAttributePair.Attribute));
        }

        [Fact]
        public void IsAssignableToParentClass()
        {
            Assert.True(_implementsAbstractAttribute.IsAssignableTo(_abstractAttributePair.OriginClass));
        }

        [Fact]
        public void MembersAsExpected()
        {
            Assert.Equal(_developerAttributePair.OriginClass.Members,
                _developerAttributePair.Attribute.Members);
        }

        [Fact]
        public void NameAsExpected()
        {
            Assert.Equal(_developerAttributePair.OriginClass.Name,
                _developerAttributePair.Attribute.Name);
        }

        [Fact]
        public void NamespaceAsExpected()
        {
            Assert.Equal(_developerAttributePair.OriginClass.Namespace,
                _developerAttributePair.Attribute.Namespace);
        }

        [Fact]
        public void NotAssignableToUnRelatedType()
        {
            Assert.False(_developerAttributePair.Attribute.IsAssignableTo(_unrelatedType));
        }

        [Fact]
        public void ParentMembersProperlyInherited()
        {
            _interfaceImplementingAttributePair.OriginClass.Members.ForEach(parentMember =>
                Assert.Contains(parentMember, _interfaceImplementingAttributePair.Attribute.MembersIncludingInherited));
        }

        [Fact]
        public void RecognizedAsAbstract()
        {
            Assert.True(_abstractAttributePair.Attribute.IsAbstract);
        }

        [Fact]
        public void RecognizedAsNotAbstract()
        {
            Assert.False(_attributeEquivalencyTestData.OriginAttribute.IsAbstract);
        }
    }
}