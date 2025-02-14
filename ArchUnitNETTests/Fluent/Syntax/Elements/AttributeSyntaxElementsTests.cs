using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class AttributeSyntaxElementsTests
    {
        public AttributeSyntaxElementsTests()
        {
            _attributes = Architecture.Attributes;
        }

        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<Attribute> _attributes;

        [Fact]
        public void AreAbstractTest()
        {
            foreach (var attribute in _attributes)
            {
                var attributeIsAbstract = Attributes().That().Are(attribute).Should().BeAbstract();
                var attributeIsNotAbstract = Attributes()
                    .That()
                    .Are(attribute)
                    .Should()
                    .NotBeAbstract();
                var abstractAttributesDoNotIncludeType = Attributes()
                    .That()
                    .AreAbstract()
                    .Should()
                    .NotBe(attribute)
                    .OrShould()
                    .NotExist();
                var notAbstractAttributesDoNotIncludeType = Attributes()
                    .That()
                    .AreNotAbstract()
                    .Should()
                    .NotBe(attribute)
                    .AndShould()
                    .Exist();

                Assert.Equal(
                    attribute.IsAbstract,
                    attributeIsAbstract.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    !attribute.IsAbstract,
                    attributeIsNotAbstract.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    !attribute.IsAbstract,
                    abstractAttributesDoNotIncludeType.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    attribute.IsAbstract,
                    notAbstractAttributesDoNotIncludeType.HasNoViolations(Architecture)
                );
            }

            var abstractAttributesAreAbstract = Attributes()
                .That()
                .AreAbstract()
                .Should()
                .BeAbstract();
            var abstractAttributesAreNotAbstract = Attributes()
                .That()
                .AreAbstract()
                .Should()
                .NotBeAbstract()
                .AndShould()
                .Exist();
            var notAbstractAttributesAreAbstract = Attributes()
                .That()
                .AreNotAbstract()
                .Should()
                .BeAbstract()
                .AndShould()
                .Exist();
            var notAbstractAttributesAreNotAbstract = Attributes()
                .That()
                .AreNotAbstract()
                .Should()
                .NotBeAbstract();

            Assert.True(abstractAttributesAreAbstract.HasNoViolations(Architecture));
            Assert.False(abstractAttributesAreNotAbstract.HasNoViolations(Architecture));
            Assert.False(notAbstractAttributesAreAbstract.HasNoViolations(Architecture));
            Assert.True(notAbstractAttributesAreNotAbstract.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreSealedTest()
        {
            foreach (var attribute in _attributes)
            {
                var attributeIsSealed = Attributes().That().Are(attribute).Should().BeSealed();
                var attributeIsNotSealed = Attributes()
                    .That()
                    .Are(attribute)
                    .Should()
                    .NotBeSealed();
                var sealedAttributesDoNotIncludeType = Attributes()
                    .That()
                    .AreSealed()
                    .Should()
                    .NotBe(attribute);
                var notSealedAttributesDoNotIncludeType = Attributes()
                    .That()
                    .AreNotSealed()
                    .Should()
                    .NotBe(attribute);

                Assert.Equal(attribute.IsSealed, attributeIsSealed.HasNoViolations(Architecture));
                Assert.Equal(
                    !attribute.IsSealed,
                    attributeIsNotSealed.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    !attribute.IsSealed,
                    sealedAttributesDoNotIncludeType.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    attribute.IsSealed,
                    notSealedAttributesDoNotIncludeType.HasNoViolations(Architecture)
                );
            }

            var sealedAttributesAreSealed = Attributes().That().AreSealed().Should().BeSealed();
            var sealedAttributesAreNotSealed = Attributes()
                .That()
                .AreSealed()
                .Should()
                .NotBeSealed()
                .AndShould()
                .Exist();
            var notSealedAttributesAreSealed = Attributes()
                .That()
                .AreNotSealed()
                .Should()
                .BeSealed()
                .AndShould()
                .Exist();
            var notSealedAttributesAreNotSealed = Attributes()
                .That()
                .AreNotSealed()
                .Should()
                .NotBeSealed();

            Assert.True(sealedAttributesAreSealed.HasNoViolations(Architecture));
            Assert.False(sealedAttributesAreNotSealed.HasNoViolations(Architecture));
            Assert.False(notSealedAttributesAreSealed.HasNoViolations(Architecture));
            Assert.True(notSealedAttributesAreNotSealed.HasNoViolations(Architecture));
        }
    }
}
