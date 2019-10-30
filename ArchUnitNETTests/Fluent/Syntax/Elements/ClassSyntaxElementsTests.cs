//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class ClassSyntaxElementsTests
    {
        public ClassSyntaxElementsTests()
        {
            _classes = Architecture.Classes;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<Class> _classes;

        [Fact]
        public void AreAbstractTest()
        {
            foreach (var cls in _classes)
            {
                var clsIsAbstract = Classes().That().Are(cls).Should().BeAbstract();
                var clsIsNotAbstract = Classes().That().Are(cls).Should().NotBeAbstract();
                var abstractClassesDoNotIncludeType = Classes().That().AreAbstract().Should().NotBe(cls);
                var notAbstractClassesDoNotIncludeType = Classes().That().AreNotAbstract().Should().NotBe(cls);

                Assert.Equal(cls.IsAbstract, clsIsAbstract.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsAbstract, clsIsNotAbstract.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsAbstract, abstractClassesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(cls.IsAbstract, notAbstractClassesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var abstractClassesAreAbstract = Classes().That().AreAbstract().Should().BeAbstract();
            var abstractClassesAreNotAbstract = Classes().That().AreAbstract().Should().NotBeAbstract();
            var notAbstractClassesAreAbstract = Classes().That().AreNotAbstract().Should().BeAbstract();
            var notAbstractClassesAreNotAbstract = Classes().That().AreNotAbstract().Should().NotBeAbstract();

            Assert.True(abstractClassesAreAbstract.HasNoViolations(Architecture));
            Assert.False(abstractClassesAreNotAbstract.HasNoViolations(Architecture));
            Assert.False(notAbstractClassesAreAbstract.HasNoViolations(Architecture));
            Assert.True(notAbstractClassesAreNotAbstract.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreEnumsTest()
        {
            foreach (var cls in _classes)
            {
                var clsIsEnum = Classes().That().Are(cls).Should().BeEnums();
                var clsIsNotEnum = Classes().That().Are(cls).Should().NotBeEnums();
                var enumClassesDoNotIncludeType = Classes().That().AreEnums().Should().NotBe(cls);
                var notEnumClassesDoNotIncludeType = Classes().That().AreNotEnums().Should().NotBe(cls);

                Assert.Equal(cls.IsEnum, clsIsEnum.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsEnum, clsIsNotEnum.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsEnum, enumClassesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(cls.IsEnum, notEnumClassesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var enumClassesAreEnums = Classes().That().AreEnums().Should().BeEnums();
            var enumClassesAreNotEnums = Classes().That().AreEnums().Should().NotBeEnums();
            var notEnumClassesAreEnums = Classes().That().AreNotEnums().Should().BeEnums();
            var notEnumClassesAreNotEnums = Classes().That().AreNotEnums().Should().NotBeEnums();

            Assert.True(enumClassesAreEnums.HasNoViolations(Architecture));
            Assert.False(enumClassesAreNotEnums.HasNoViolations(Architecture));
            Assert.False(notEnumClassesAreEnums.HasNoViolations(Architecture));
            Assert.True(notEnumClassesAreNotEnums.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreSealedTest()
        {
            foreach (var cls in _classes)
            {
                var clsIsSealed = Classes().That().Are(cls).Should().BeSealed();
                var clsIsNotSealed = Classes().That().Are(cls).Should().NotBeSealed();
                var sealedClassesDoNotIncludeType = Classes().That().AreSealed().Should().NotBe(cls);
                var notSealedClassesDoNotIncludeType = Classes().That().AreNotSealed().Should().NotBe(cls);

                Assert.Equal(cls.IsSealed, clsIsSealed.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsSealed, clsIsNotSealed.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsSealed, sealedClassesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(cls.IsSealed, notSealedClassesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var sealedClassesAreSealed = Classes().That().AreSealed().Should().BeSealed();
            var sealedClassesAreNotSealed = Classes().That().AreSealed().Should().NotBeSealed();
            var notSealedClassesAreSealed = Classes().That().AreNotSealed().Should().BeSealed();
            var notSealedClassesAreNotSealed = Classes().That().AreNotSealed().Should().NotBeSealed();

            Assert.True(sealedClassesAreSealed.HasNoViolations(Architecture));
            Assert.False(sealedClassesAreNotSealed.HasNoViolations(Architecture));
            Assert.False(notSealedClassesAreSealed.HasNoViolations(Architecture));
            Assert.True(notSealedClassesAreNotSealed.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreStructsTest()
        {
            foreach (var cls in _classes)
            {
                var clsIsStruct = Classes().That().Are(cls).Should().BeStructs();
                var clsIsNotStruct = Classes().That().Are(cls).Should().NotBeStructs();
                var structClassesDoNotIncludeType = Classes().That().AreStructs().Should().NotBe(cls);
                var notStructClassesDoNotIncludeType = Classes().That().AreNotStructs().Should().NotBe(cls);

                Assert.Equal(cls.IsStruct, clsIsStruct.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsStruct, clsIsNotStruct.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsStruct, structClassesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(cls.IsStruct, notStructClassesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var structClassesAreStructs = Classes().That().AreStructs().Should().BeStructs();
            var structClassesAreNotStructs = Classes().That().AreStructs().Should().NotBeStructs();
            var notStructClassesAreStructs = Classes().That().AreNotStructs().Should().BeStructs();
            var notStructClassesAreNotStructs = Classes().That().AreNotStructs().Should().NotBeStructs();

            Assert.True(structClassesAreStructs.HasNoViolations(Architecture));
            Assert.False(structClassesAreNotStructs.HasNoViolations(Architecture));
            Assert.False(notStructClassesAreStructs.HasNoViolations(Architecture));
            Assert.True(notStructClassesAreNotStructs.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreValueTypesTest()
        {
            foreach (var cls in _classes)
            {
                var clsIsValueType = Classes().That().Are(cls).Should().BeValueTypes();
                var clsIsNotValueType = Classes().That().Are(cls).Should().NotBeValueTypes();
                var valueTypeClassesDoNotIncludeType = Classes().That().AreValueTypes().Should().NotBe(cls);
                var notValueTypeClassesDoNotIncludeType = Classes().That().AreNotValueTypes().Should().NotBe(cls);

                Assert.Equal(cls.IsValueType, clsIsValueType.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsValueType, clsIsNotValueType.HasNoViolations(Architecture));
                Assert.Equal(!cls.IsValueType, valueTypeClassesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(cls.IsValueType, notValueTypeClassesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var valueTypeClassesAreValueTypes = Classes().That().AreValueTypes().Should().BeValueTypes();
            var valueTypeClassesAreNotValueTypes = Classes().That().AreValueTypes().Should().NotBeValueTypes();
            var notValueTypeClassesAreValueTypes = Classes().That().AreNotValueTypes().Should().BeValueTypes();
            var notValueTypeClassesAreNotValueTypes = Classes().That().AreNotValueTypes().Should().NotBeValueTypes();

            Assert.True(valueTypeClassesAreValueTypes.HasNoViolations(Architecture));
            Assert.False(valueTypeClassesAreNotValueTypes.HasNoViolations(Architecture));
            Assert.False(notValueTypeClassesAreValueTypes.HasNoViolations(Architecture));
            Assert.True(notValueTypeClassesAreNotValueTypes.HasNoViolations(Architecture));
        }
    }
}