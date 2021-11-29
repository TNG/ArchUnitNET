//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
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
            Assert.True(Classes().That().AreNotEnums().Should().Be(Classes()).HasNoViolations(Architecture));
            Assert.True(Classes().That().AreEnums().Should().NotExist().HasNoViolations(Architecture));
            Assert.True(Classes().Should().NotBeEnums().HasNoViolations(Architecture));
            Assert.False(Classes().Should().BeEnums().HasNoViolations(Architecture));
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
            Assert.True(Classes().That().AreNotStructs().Should().Be(Classes()).HasNoViolations(Architecture));
            Assert.True(Classes().That().AreStructs().Should().NotExist().HasNoViolations(Architecture));
            Assert.True(Classes().Should().NotBeStructs().HasNoViolations(Architecture));
            Assert.False(Classes().Should().BeStructs().HasNoViolations(Architecture));
        }

        [Fact]
        public void AreValueTypesTest()
        {
            Assert.True(Classes().That().AreNotValueTypes().Should().Be(Classes()).HasNoViolations(Architecture));
            Assert.True(Classes().That().AreValueTypes().Should().NotExist().HasNoViolations(Architecture));
            Assert.True(Classes().Should().NotBeValueTypes().HasNoViolations(Architecture));
            Assert.False(Classes().Should().BeValueTypes().HasNoViolations(Architecture));
        }
    }
}