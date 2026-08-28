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

        // Because() and As() are only exercised for Classes and Attributes above, but they are created
        // reflectively per rule type via ConjunctionFactory.Create(Activator.CreateInstance), so a wrong
        // constructor signature on any other "...ConjunctionWithDescription" fails at runtime, not compile
        // time. Cover the remaining rule types here.
        [Fact]
        public void CustomDescriptionIsSupportedForEveryRuleType()
        {
            Assert.Equal(
                CustomDescription,
                Types()
                    .That()
                    .FollowCustomPredicate(t => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Types " + CustomDescription,
                Types().Should().Exist().As(CustomDescription).Description
            );

            Assert.Equal(
                CustomDescription,
                Interfaces()
                    .That()
                    .FollowCustomPredicate(i => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Interfaces " + CustomDescription,
                Interfaces().Should().Exist().As(CustomDescription).Description
            );

            // Attributes()'s condition-side "...ShouldConjunctionWithDescription" is already covered by
            // _customDescriptionTestRule1 and _combinedCustomDescriptionTestRule above; only the
            // predicate-side one is otherwise unreachable.
            Assert.Equal(
                CustomDescription,
                Attributes()
                    .That()
                    .FollowCustomPredicate(a => true, "exist")
                    .As(CustomDescription)
                    .Description
            );

            Assert.Equal(
                CustomDescription,
                Members()
                    .That()
                    .FollowCustomPredicate(m => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Members " + CustomDescription,
                Members().Should().Exist().As(CustomDescription).Description
            );

            Assert.Equal(
                CustomDescription,
                MethodMembers()
                    .That()
                    .FollowCustomPredicate(m => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Method members " + CustomDescription,
                MethodMembers().Should().Exist().As(CustomDescription).Description
            );

            Assert.Equal(
                CustomDescription,
                PropertyMembers()
                    .That()
                    .FollowCustomPredicate(p => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Property members " + CustomDescription,
                PropertyMembers().Should().Exist().As(CustomDescription).Description
            );

            Assert.Equal(
                CustomDescription,
                FieldMembers()
                    .That()
                    .FollowCustomPredicate(f => true, "exist")
                    .As(CustomDescription)
                    .Description
            );
            Assert.Equal(
                "Field members " + CustomDescription,
                FieldMembers().Should().Exist().As(CustomDescription).Description
            );
        }

        [Fact]
        public void BecauseIsSupportedForEveryRuleType()
        {
            Assert.Equal(
                "Types that exist because reason",
                Types()
                    .That()
                    .FollowCustomPredicate(t => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Types should exist because reason",
                Types().Should().Exist().Because("reason").Description
            );

            Assert.Equal(
                "Interfaces that exist because reason",
                Interfaces()
                    .That()
                    .FollowCustomPredicate(i => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Interfaces should exist because reason",
                Interfaces().Should().Exist().Because("reason").Description
            );

            Assert.Equal(
                "Attributes that exist because reason",
                Attributes()
                    .That()
                    .FollowCustomPredicate(a => true, "exist")
                    .Because("reason")
                    .Description
            );

            Assert.Equal(
                "Members that exist because reason",
                Members()
                    .That()
                    .FollowCustomPredicate(m => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Members should exist because reason",
                Members().Should().Exist().Because("reason").Description
            );

            Assert.Equal(
                "Method members that exist because reason",
                MethodMembers()
                    .That()
                    .FollowCustomPredicate(m => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Method members should exist because reason",
                MethodMembers().Should().Exist().Because("reason").Description
            );

            Assert.Equal(
                "Property members that exist because reason",
                PropertyMembers()
                    .That()
                    .FollowCustomPredicate(p => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Property members should exist because reason",
                PropertyMembers().Should().Exist().Because("reason").Description
            );

            Assert.Equal(
                "Field members that exist because reason",
                FieldMembers()
                    .That()
                    .FollowCustomPredicate(f => true, "exist")
                    .Because("reason")
                    .Description
            );
            Assert.Equal(
                "Field members should exist because reason",
                FieldMembers().Should().Exist().Because("reason").Description
            );
        }
    }
}
