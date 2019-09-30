using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class MethodMemberSyntaxElementsTests
    {
        public MethodMemberSyntaxElementsTests()
        {
            _methodMembers = Architecture.MethodMembers;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<MethodMember> _methodMembers;

        [Fact]
        public void AreConstructorsTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                var methodMemberIsConstructor = MethodMembers().That().Are(methodMember).Should().BeConstructor();
                var methodMemberIsNoConstructor = MethodMembers().That().Are(methodMember).Should().BeNoConstructor();
                var constructorMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreConstructors().Should().NotBe(methodMember).OrShould().NotExist();
                var noConstructorMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreNoConstructors().Should().NotBe(methodMember).AndShould().Exist();

                Assert.Equal(methodMember.IsConstructor(), methodMemberIsConstructor.HasViolations(Architecture));
                Assert.Equal(!methodMember.IsConstructor(), methodMemberIsNoConstructor.HasViolations(Architecture));
                Assert.Equal(!methodMember.IsConstructor(),
                    constructorMethodMembersDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(methodMember.IsConstructor(),
                    noConstructorMethodMembersDoNotIncludeMember.HasViolations(Architecture));
            }

            var constructorMethodMembersShouldBeConstructor =
                MethodMembers().That().AreConstructors().Should().BeConstructor();
            var constructorMethodMembersAreNoConstructors =
                MethodMembers().That().AreConstructors().Should().BeNoConstructor().AndShould().Exist();
            var noConstructorMethodMembersShouldBeConstructor =
                MethodMembers().That().AreNoConstructors().Should().BeConstructor().AndShould().Exist();
            var noConstructorMethodMembersAreNoConstructors =
                MethodMembers().That().AreNoConstructors().Should().BeNoConstructor();

            Assert.True(constructorMethodMembersShouldBeConstructor.HasViolations(Architecture));
            Assert.False(constructorMethodMembersAreNoConstructors.HasViolations(Architecture));
            Assert.False(noConstructorMethodMembersShouldBeConstructor.HasViolations(Architecture));
            Assert.True(noConstructorMethodMembersAreNoConstructors.HasViolations(Architecture));
        }

        [Fact]
        public void AreVirtualTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                var methodMemberIsVirtual = MethodMembers().That().Are(methodMember).Should().BeVirtual();
                var methodMemberIsNotVirtual = MethodMembers().That().Are(methodMember).Should().NotBeVirtual();
                var virtualMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreVirtual().Should().NotBe(methodMember).OrShould().NotExist();
                var notVirtualMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreNotVirtual().Should().NotBe(methodMember).AndShould().Exist();

                Assert.Equal(methodMember.IsVirtual, methodMemberIsVirtual.HasViolations(Architecture));
                Assert.Equal(!methodMember.IsVirtual, methodMemberIsNotVirtual.HasViolations(Architecture));
                Assert.Equal(!methodMember.IsVirtual,
                    virtualMethodMembersDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(methodMember.IsVirtual,
                    notVirtualMethodMembersDoNotIncludeMember.HasViolations(Architecture));
            }

            var virtualMethodMembersShouldBeVirtual = MethodMembers().That().AreVirtual().Should().BeVirtual();
            var virtualMethodMembersAreNotVirtual =
                MethodMembers().That().AreVirtual().Should().NotBeVirtual().AndShould().Exist();
            var notVirtualMethodMembersShouldBeVirtual =
                MethodMembers().That().AreNotVirtual().Should().BeVirtual().AndShould().Exist();
            var notVirtualMethodMembersAreNotVirtual =
                MethodMembers().That().AreNotVirtual().Should().NotBeVirtual();

            Assert.True(virtualMethodMembersShouldBeVirtual.HasViolations(Architecture));
            Assert.False(virtualMethodMembersAreNotVirtual.HasViolations(Architecture));
            Assert.False(notVirtualMethodMembersShouldBeVirtual.HasViolations(Architecture));
            Assert.True(notVirtualMethodMembersAreNotVirtual.HasViolations(Architecture));
        }
    }
}