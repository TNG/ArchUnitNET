using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class DescriptionTests
    {
        [Fact]
        public void DescriptionTest()
        {
            var descriptionTestRule = Classes().That().HaveNameStartingWith("test").And().AreNotNested().Should()
                .BeAbstract().OrShould().NotBeAbstract().And(Types().Should().BeNested()).Or().Attributes().Should()
                .Exist();
            const string expectedDescription =
                "Classes that have name starting with \"test\" and are not nested should be abstract or should not be abstract and Types should be nested or Attributes should exist";
            Assert.Equal(expectedDescription, descriptionTestRule.Description);
            Assert.Equal(expectedDescription, descriptionTestRule.ToString());
        }
    }
}