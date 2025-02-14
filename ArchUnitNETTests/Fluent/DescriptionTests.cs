using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class DescriptionTests
    {
        private readonly IArchRule _descriptionTestRule = Classes()
            .That()
            .HaveNameStartingWith("test")
            .Because("reason1")
            .And()
            .AreNotNested()
            .Should()
            .BeAbstract()
            .Because("reason2")
            .OrShould()
            .NotBeAbstract()
            .And(Types().Should().BeNested())
            .Or()
            .Attributes()
            .Should()
            .Exist()
            .Because("reason3");

        private readonly IArchRule _customDescriptionTestRule1 = Classes()
            .Should()
            .BeAbstract()
            .As(CustomDescription);

        private readonly IArchRule _customDescriptionTestRule2 = Classes()
            .That()
            .ArePublic()
            .As(CustomDescription)
            .And()
            .AreProtected()
            .Should()
            .BePublic()
            .AndShould()
            .BeAbstract()
            .As(CustomDescription);

        private readonly IArchRule _combinedCustomDescriptionTestRule = Classes()
            .Should()
            .BeAbstract()
            .As(CustomDescription)
            .And()
            .Attributes()
            .Should()
            .BeAbstract()
            .As(CustomDescription);

        private const string ExpectedDescription =
            "Classes that have name starting with \"test\" because reason1 and are not nested should be abstract because reason2 or should not be abstract and Types should be nested or Attributes should exist because reason3";

        private const string CustomDescription = "custom description";

        [Fact]
        public void CustomDescriptionTest()
        {
            Assert.Equal("Classes " + CustomDescription, _customDescriptionTestRule1.Description);
            Assert.Equal("Classes " + CustomDescription, _customDescriptionTestRule1.ToString());
            Assert.Equal(
                CustomDescription + " and are protected " + CustomDescription,
                _customDescriptionTestRule2.Description
            );
            Assert.Equal(
                CustomDescription + " and are protected " + CustomDescription,
                _customDescriptionTestRule2.ToString()
            );
            Assert.Equal(
                "Classes " + CustomDescription + " and Attributes " + CustomDescription,
                _combinedCustomDescriptionTestRule.Description
            );
            Assert.Equal(
                "Classes " + CustomDescription + " and Attributes " + CustomDescription,
                _combinedCustomDescriptionTestRule.ToString()
            );
        }

        [Fact]
        public void DescriptionTest()
        {
            Assert.Equal(ExpectedDescription, _descriptionTestRule.Description);
            Assert.Equal(ExpectedDescription, _descriptionTestRule.ToString());
        }
    }
}
