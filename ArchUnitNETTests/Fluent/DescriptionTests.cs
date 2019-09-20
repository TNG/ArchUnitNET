using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class DescriptionTests
    {
        private readonly IArchRule _descriptionTestRule = Classes().That().HaveNameStartingWith("test")
            .Because("reason1").And().AreNotNested().Should().BeAbstract().Because("reason2").OrShould().NotBeAbstract()
            .And(Types().Should().BeNested()).Or().Attributes().Should().Exist().Because("reason3");

        private const string ExpectedDescription =
            "Classes that have name starting with \"test\" because reason1 and are not nested should be abstract because reason2 or should not be abstract and Types should be nested or Attributes should exist because reason3";

        [Fact]
        public void DescriptionTest()
        {
            Assert.Equal(ExpectedDescription, _descriptionTestRule.Description);
            Assert.Equal(ExpectedDescription, _descriptionTestRule.ToString());
        }
    }
}