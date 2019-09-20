﻿using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
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

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<Attribute> _attributes;

        [Fact]
        public void AreAbstractTest()
        {
            foreach (var attribute in _attributes)
            {
                var attributeIsAbstract = Attributes().That().Are(attribute).Should().BeAbstract();
                var attributeIsNotAbstract = Attributes().That().Are(attribute).Should().NotBeAbstract();
                var abstractAttributesDoNotIncludeType =
                    Attributes().That().AreAbstract().Should().NotBe(attribute).OrShould().NotExist();
                var notAbstractAttributesDoNotIncludeType = Attributes().That().AreNotAbstract().Should()
                    .NotBe(attribute).AndShould().Exist();

                Assert.Equal(attribute.IsAbstract, attributeIsAbstract.Check(Architecture));
                Assert.Equal(!attribute.IsAbstract, attributeIsNotAbstract.Check(Architecture));
                Assert.Equal(!attribute.IsAbstract, abstractAttributesDoNotIncludeType.Check(Architecture));
                Assert.Equal(attribute.IsAbstract, notAbstractAttributesDoNotIncludeType.Check(Architecture));
            }

            var abstractAttributesAreAbstract = Attributes().That().AreAbstract().Should().BeAbstract();
            var abstractAttributesAreNotAbstract =
                Attributes().That().AreAbstract().Should().NotBeAbstract().AndShould().Exist();
            var notAbstractAttributesAreAbstract =
                Attributes().That().AreNotAbstract().Should().BeAbstract().AndShould().Exist();
            var notAbstractAttributesAreNotAbstract = Attributes().That().AreNotAbstract().Should().NotBeAbstract();

            Assert.True(abstractAttributesAreAbstract.Check(Architecture));
            Assert.False(abstractAttributesAreNotAbstract.Check(Architecture));
            Assert.False(notAbstractAttributesAreAbstract.Check(Architecture));
            Assert.True(notAbstractAttributesAreNotAbstract.Check(Architecture));
        }
    }
}