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
    }
}