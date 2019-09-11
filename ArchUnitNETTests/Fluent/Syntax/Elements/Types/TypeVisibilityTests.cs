using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;

namespace ArchUnitNETTests.Fluent.Syntax.Elements.Types
{
    /// <summary>
    ///     These tests should be true for all Architectures.
    /// </summary>
    public class TypeVisibilityTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                ArchRuleDefinition.Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            Assert.True(typesThatAreNotNestedMustBeVisible.Check(Architecture));
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = ArchRuleDefinition.Types().That().ArePrivate().Or()
                .AreProtected().Or()
                .ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            Assert.True(typesWithRestrictedVisibilityMustBeNested.Check(Architecture));
        }
    }
}