using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent.Syntax;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class ArchRuleEqualityTests
    {
        private readonly IArchRule _rule = Members()
            .That()
            .ArePrivate()
            .Should()
            .BePrivate()
            .And(Classes().Should().BeAbstract().Or().Attributes().Should().Exist())
            .And(Types().Should().BeNested());

        private readonly IArchRule _equalRule = Members()
            .That()
            .ArePrivate()
            .Should()
            .BePrivate()
            .And(Classes().Should().BeAbstract().Or().Attributes().Should().Exist())
            .And(Types().Should().BeNested());

        private readonly IArchRule _notEqualRule = Members()
            .That()
            .ArePrivate()
            .Should()
            .BePrivate()
            .And(Classes().Should().BeAbstract())
            .Or()
            .Attributes()
            .Should()
            .Exist()
            .And(Types().Should().BeNested());

        [Fact]
        public void ArchRuleEqualityTest()
        {
            Assert.Equal(_rule, _equalRule);
            Assert.NotEqual(_rule, _notEqualRule);
            Assert.NotEqual(_equalRule, _notEqualRule);
        }
    }
}
