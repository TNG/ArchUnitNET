using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class DescriptionTests
    {
        private readonly IArchRule _descriptionTestRule = Classes().That().HaveNameStartingWith("test").And().AreNotNested()
            .Should().BeAbstract().OrShould().NotBeAbstract().And(Types().Should().BeNested()).Or().Attributes()
            .Should().Exist();

        private const string ExpectedDescription =
            "Classes that have name starting with \"test\" and are not nested should be abstract or should not be abstract and Types should be nested or Attributes should exist";

        [Fact]
        public void DescriptionTest()
        {
            Assert.Equal(ExpectedDescription, _descriptionTestRule.Description);
            Assert.Equal(ExpectedDescription, _descriptionTestRule.ToString());
        }
    }
}