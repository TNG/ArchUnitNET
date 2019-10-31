//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class ArchRuleEqualityTests
    {
        private readonly IArchRule _rule = Members().That().ArePrivate().Should().BePrivate()
            .And(Classes().Should().BeAbstract().Or().Attributes().Should().Exist()).And(Types().Should().BeNested());

        private readonly IArchRule _equalRule = Members().That().ArePrivate().Should().BePrivate()
            .And(Classes().Should().BeAbstract().Or().Attributes().Should().Exist()).And(Types().Should().BeNested());

        private readonly IArchRule _notEqualRule = Members().That().ArePrivate().Should().BePrivate()
            .And(Classes().Should().BeAbstract()).Or().Attributes().Should().Exist().And(Types().Should().BeNested());

        [Fact]
        public void ArchRuleEqualityTest()
        {
            Assert.Equal(_rule, _equalRule);
            Assert.NotEqual(_rule, _notEqualRule);
            Assert.NotEqual(_equalRule, _notEqualRule);
        }
    }
}