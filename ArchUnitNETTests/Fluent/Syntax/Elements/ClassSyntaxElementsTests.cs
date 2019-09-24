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

                Assert.Equal(cls.IsAbstract, clsIsAbstract.Check(Architecture));
                Assert.Equal(!cls.IsAbstract, clsIsNotAbstract.Check(Architecture));
                Assert.Equal(!cls.IsAbstract, abstractClassesDoNotIncludeType.Check(Architecture));
                Assert.Equal(cls.IsAbstract, notAbstractClassesDoNotIncludeType.Check(Architecture));
            }

            var abstractClassesAreAbstract = Classes().That().AreAbstract().Should().BeAbstract();
            var abstractClassesAreNotAbstract = Classes().That().AreAbstract().Should().NotBeAbstract();
            var notAbstractClassesAreAbstract = Classes().That().AreNotAbstract().Should().BeAbstract();
            var notAbstractClassesAreNotAbstract = Classes().That().AreNotAbstract().Should().NotBeAbstract();

            Assert.True(abstractClassesAreAbstract.Check(Architecture));
            Assert.False(abstractClassesAreNotAbstract.Check(Architecture));
            Assert.False(notAbstractClassesAreAbstract.Check(Architecture));
            Assert.True(notAbstractClassesAreNotAbstract.Check(Architecture));
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

                Assert.Equal(cls.IsEnum, clsIsEnum.Check(Architecture));
                Assert.Equal(!cls.IsEnum, clsIsNotEnum.Check(Architecture));
                Assert.Equal(!cls.IsEnum, enumClassesDoNotIncludeType.Check(Architecture));
                Assert.Equal(cls.IsEnum, notEnumClassesDoNotIncludeType.Check(Architecture));
            }

            var enumClassesAreEnums = Classes().That().AreEnums().Should().BeEnums();
            var enumClassesAreNotEnums = Classes().That().AreEnums().Should().NotBeEnums();
            var notEnumClassesAreEnums = Classes().That().AreNotEnums().Should().BeEnums();
            var notEnumClassesAreNotEnums = Classes().That().AreNotEnums().Should().NotBeEnums();

            Assert.True(enumClassesAreEnums.Check(Architecture));
            Assert.False(enumClassesAreNotEnums.Check(Architecture));
            Assert.False(notEnumClassesAreEnums.Check(Architecture));
            Assert.True(notEnumClassesAreNotEnums.Check(Architecture));
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

                Assert.Equal(cls.IsSealed, clsIsSealed.Check(Architecture));
                Assert.Equal(!cls.IsSealed, clsIsNotSealed.Check(Architecture));
                Assert.Equal(!cls.IsSealed, sealedClassesDoNotIncludeType.Check(Architecture));
                Assert.Equal(cls.IsSealed, notSealedClassesDoNotIncludeType.Check(Architecture));
            }

            var sealedClassesAreSealed = Classes().That().AreSealed().Should().BeSealed();
            var sealedClassesAreNotSealed = Classes().That().AreSealed().Should().NotBeSealed();
            var notSealedClassesAreSealed = Classes().That().AreNotSealed().Should().BeSealed();
            var notSealedClassesAreNotSealed = Classes().That().AreNotSealed().Should().NotBeSealed();

            Assert.True(sealedClassesAreSealed.Check(Architecture));
            Assert.False(sealedClassesAreNotSealed.Check(Architecture));
            Assert.False(notSealedClassesAreSealed.Check(Architecture));
            Assert.True(notSealedClassesAreNotSealed.Check(Architecture));
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

                Assert.Equal(cls.IsStruct, clsIsStruct.Check(Architecture));
                Assert.Equal(!cls.IsStruct, clsIsNotStruct.Check(Architecture));
                Assert.Equal(!cls.IsStruct, structClassesDoNotIncludeType.Check(Architecture));
                Assert.Equal(cls.IsStruct, notStructClassesDoNotIncludeType.Check(Architecture));
            }

            var structClassesAreStructs = Classes().That().AreStructs().Should().BeStructs();
            var structClassesAreNotStructs = Classes().That().AreStructs().Should().NotBeStructs();
            var notStructClassesAreStructs = Classes().That().AreNotStructs().Should().BeStructs();
            var notStructClassesAreNotStructs = Classes().That().AreNotStructs().Should().NotBeStructs();

            Assert.True(structClassesAreStructs.Check(Architecture));
            Assert.False(structClassesAreNotStructs.Check(Architecture));
            Assert.False(notStructClassesAreStructs.Check(Architecture));
            Assert.True(notStructClassesAreNotStructs.Check(Architecture));
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

                Assert.Equal(cls.IsValueType, clsIsValueType.Check(Architecture));
                Assert.Equal(!cls.IsValueType, clsIsNotValueType.Check(Architecture));
                Assert.Equal(!cls.IsValueType, valueTypeClassesDoNotIncludeType.Check(Architecture));
                Assert.Equal(cls.IsValueType, notValueTypeClassesDoNotIncludeType.Check(Architecture));
            }

            var valueTypeClassesAreValueTypes = Classes().That().AreValueTypes().Should().BeValueTypes();
            var valueTypeClassesAreNotValueTypes = Classes().That().AreValueTypes().Should().NotBeValueTypes();
            var notValueTypeClassesAreValueTypes = Classes().That().AreNotValueTypes().Should().BeValueTypes();
            var notValueTypeClassesAreNotValueTypes = Classes().That().AreNotValueTypes().Should().NotBeValueTypes();

            Assert.True(valueTypeClassesAreValueTypes.Check(Architecture));
            Assert.False(valueTypeClassesAreNotValueTypes.Check(Architecture));
            Assert.False(notValueTypeClassesAreValueTypes.Check(Architecture));
            Assert.True(notValueTypeClassesAreNotValueTypes.Check(Architecture));
        }
    }
}