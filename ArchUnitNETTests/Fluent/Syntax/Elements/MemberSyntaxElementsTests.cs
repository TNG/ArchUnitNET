using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class MemberSyntaxElementsTests
    {
        public MemberSyntaxElementsTests()
        {
            _members = Architecture.Members;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IMember> _members;

        [Fact]
        public void HaveBodyTypeMemberDependencies()
        {
            foreach (var member in _members)
            {
                foreach (var dependency in member.GetBodyTypeMemberDependencies())
                {
                    var memberHasDependency =
                        Members().That().Are(member).Should().HaveBodyTypeMemberDependencies(dependency.Origin.Name)
                            .OrShould().HaveBodyTypeMemberDependencies(dependency.Target.Name);
                    var memberDoesNotHaveDependency = Members().That().Are(member).Should()
                        .NotHaveBodyTypeMemberDependencies(dependency.Origin.Name).AndShould()
                        .NotHaveBodyTypeMemberDependencies(dependency.Target.Name);
                    var membersWithDependencyShouldExist =
                        Members().That().HaveBodyTypeMemberDependencies(dependency.Origin.Name).Or()
                            .HaveBodyTypeMemberDependencies(dependency.Target.Name).Should().Exist();
                    var membersWithDependencyShouldBeOtherMembers = Members().That()
                        .HaveBodyTypeMemberDependencies(dependency.Origin.Name).Or()
                        .HaveBodyTypeMemberDependencies(dependency.Target.Name).Should().NotBe(member);
                    var membersWithDependencyShouldHaveDependency = Members().That()
                        .HaveBodyTypeMemberDependencies(dependency.Origin.Name).Should()
                        .HaveBodyTypeMemberDependencies(dependency.Origin.Name);
                    var membersWithDependencyExist =
                        Members().That().HaveBodyTypeMemberDependencies(dependency.Origin.Name).Should().Exist();

                    Assert.True(memberHasDependency.HasViolations(Architecture));
                    Assert.False(memberDoesNotHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldExist.HasViolations(Architecture));
                    Assert.False(membersWithDependencyShouldBeOtherMembers.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyExist.HasViolations(Architecture));
                }

                var memberHasBodyTypeMemberDependencies =
                    Members().That().Are(member).Should().HaveBodyTypeMemberDependencies();
                var memberDoesNotHaveBodyTypeMemberDependencies =
                    Members().That().Are(member).Should().NotHaveBodyTypeMemberDependencies();
                var membersWithBodyTypeMemberDependenciesDoNotIncludeMember =
                    Members().That().HaveBodyTypeMemberDependencies().Should().NotBe(member);
                var membersWithoutBodyTypeMemberDependenciesDoNotIncludeMember =
                    Members().That().DoNotHaveBodyTypeMemberDependencies().Should().NotBe(member);

                Assert.Equal(member.HasBodyTypeMemberDependencies(),
                    memberHasBodyTypeMemberDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasBodyTypeMemberDependencies(),
                    memberDoesNotHaveBodyTypeMemberDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasBodyTypeMemberDependencies(),
                    membersWithBodyTypeMemberDependenciesDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(member.HasBodyTypeMemberDependencies(),
                    membersWithoutBodyTypeMemberDependenciesDoNotIncludeMember.HasViolations(Architecture));
            }

            var membersWithBodyTypeMemberDependenciesHaveBodyTypeMemberDependencies = Members().That()
                .HaveBodyTypeMemberDependencies().Should().HaveBodyTypeMemberDependencies();
            var membersWithBodyTypeMemberDependenciesDoNotHaveBodyTypeMemberDependencies = Members().That()
                .HaveBodyTypeMemberDependencies().Should().NotHaveBodyTypeMemberDependencies().AndShould().Exist();
            var membersWithoutBodyTypeMemberDependenciesHaveBodyTypeMemberDependencies = Members().That()
                .DoNotHaveBodyTypeMemberDependencies().Should().HaveBodyTypeMemberDependencies().AndShould().Exist();
            var membersWithoutBodyTypeMemberDependenciesDoNotHaveBodyTypeMemberDependencies = Members().That()
                .DoNotHaveBodyTypeMemberDependencies().Should().NotHaveBodyTypeMemberDependencies();

            Assert.True(
                membersWithBodyTypeMemberDependenciesHaveBodyTypeMemberDependencies.HasViolations(Architecture));
            Assert.False(
                membersWithBodyTypeMemberDependenciesDoNotHaveBodyTypeMemberDependencies.HasViolations(Architecture));
            Assert.False(
                membersWithoutBodyTypeMemberDependenciesHaveBodyTypeMemberDependencies.HasViolations(Architecture));
            Assert.True(
                membersWithoutBodyTypeMemberDependenciesDoNotHaveBodyTypeMemberDependencies
                    .HasViolations(Architecture));
        }

        [Fact]
        public void HaveFieldTypeDependencies()
        {
            foreach (var member in _members)
            {
                foreach (var dependency in member.GetFieldTypeDependencies())
                {
                    var memberHasDependency =
                        Members().That().Are(member).Should().HaveFieldTypeDependencies(dependency.Origin.Name)
                            .OrShould().HaveFieldTypeDependencies(dependency.Target.Name);
                    var memberDoesNotHaveDependency = Members().That().Are(member).Should()
                        .NotHaveFieldTypeDependencies(dependency.Origin.Name).AndShould()
                        .NotHaveFieldTypeDependencies(dependency.Target.Name);
                    var membersWithDependencyShouldExist =
                        Members().That().HaveFieldTypeDependencies(dependency.Origin.Name).Or()
                            .HaveFieldTypeDependencies(dependency.Target.Name).Should().Exist();
                    var membersWithDependencyShouldBeOtherMembers = Members().That()
                        .HaveFieldTypeDependencies(dependency.Origin.Name).Or()
                        .HaveFieldTypeDependencies(dependency.Target.Name).Should().NotBe(member);
                    var membersWithDependencyShouldHaveDependency = Members().That()
                        .HaveFieldTypeDependencies(dependency.Origin.Name).Should()
                        .HaveFieldTypeDependencies(dependency.Origin.Name);
                    var membersWithDependencyExist =
                        Members().That().HaveFieldTypeDependencies(dependency.Origin.Name).Should().Exist();

                    Assert.True(memberHasDependency.HasViolations(Architecture));
                    Assert.False(memberDoesNotHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldExist.HasViolations(Architecture));
                    Assert.False(membersWithDependencyShouldBeOtherMembers.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyExist.HasViolations(Architecture));
                }

                var memberHasFieldTypeDependencies =
                    Members().That().Are(member).Should().HaveFieldTypeDependencies();
                var memberDoesNotHaveFieldTypeDependencies =
                    Members().That().Are(member).Should().NotHaveFieldTypeDependencies();
                var membersWithFieldTypeDependenciesDoNotIncludeMember =
                    Members().That().HaveFieldTypeDependencies().Should().NotBe(member);
                var membersWithoutFieldTypeDependenciesDoNotIncludeMember =
                    Members().That().DoNotHaveFieldTypeDependencies().Should().NotBe(member);

                Assert.Equal(member.HasFieldTypeDependencies(),
                    memberHasFieldTypeDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasFieldTypeDependencies(),
                    memberDoesNotHaveFieldTypeDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasFieldTypeDependencies(),
                    membersWithFieldTypeDependenciesDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(member.HasFieldTypeDependencies(),
                    membersWithoutFieldTypeDependenciesDoNotIncludeMember.HasViolations(Architecture));
            }

            var membersWithFieldTypeDependenciesHaveFieldTypeDependencies = Members().That()
                .HaveFieldTypeDependencies().Should().HaveFieldTypeDependencies();
            var membersWithFieldTypeDependenciesDoNotHaveFieldTypeDependencies = Members().That()
                .HaveFieldTypeDependencies().Should().NotHaveFieldTypeDependencies().AndShould().Exist();
            var membersWithoutFieldTypeDependenciesHaveFieldTypeDependencies = Members().That()
                .DoNotHaveFieldTypeDependencies().Should().HaveFieldTypeDependencies().AndShould().Exist();
            var membersWithoutFieldTypeDependenciesDoNotHaveFieldTypeDependencies = Members().That()
                .DoNotHaveFieldTypeDependencies().Should().NotHaveFieldTypeDependencies();

            Assert.True(membersWithFieldTypeDependenciesHaveFieldTypeDependencies.HasViolations(Architecture));
            Assert.False(membersWithFieldTypeDependenciesDoNotHaveFieldTypeDependencies.HasViolations(Architecture));
            Assert.False(membersWithoutFieldTypeDependenciesHaveFieldTypeDependencies.HasViolations(Architecture));
            Assert.True(
                membersWithoutFieldTypeDependenciesDoNotHaveFieldTypeDependencies.HasViolations(Architecture));
        }

        [Fact]
        public void HaveMethodCallDependencies()
        {
            foreach (var member in _members)
            {
                foreach (var dependency in member.GetMethodCallDependencies())
                {
                    var memberHasDependency =
                        Members().That().Are(member).Should().HaveMethodCallDependencies(dependency.Origin.Name)
                            .OrShould().HaveMethodCallDependencies(dependency.Target.Name);
                    var memberDoesNotHaveDependency = Members().That().Are(member).Should()
                        .NotHaveMethodCallDependencies(dependency.Origin.Name).AndShould()
                        .NotHaveMethodCallDependencies(dependency.Target.Name);
                    var membersWithDependencyShouldExist =
                        Members().That().HaveMethodCallDependencies(dependency.Origin.Name).Or()
                            .HaveMethodCallDependencies(dependency.Target.Name).Should().Exist();
                    var membersWithDependencyShouldBeOtherMembers = Members().That()
                        .HaveMethodCallDependencies(dependency.Origin.Name).Or()
                        .HaveMethodCallDependencies(dependency.Target.Name).Should().NotBe(member);
                    var membersWithDependencyShouldHaveDependency = Members().That()
                        .HaveMethodCallDependencies(dependency.Origin.Name).Should()
                        .HaveMethodCallDependencies(dependency.Origin.Name);
                    var membersWithDependencyExist =
                        Members().That().HaveMethodCallDependencies(dependency.Origin.Name).Should().Exist();

                    Assert.True(memberHasDependency.HasViolations(Architecture));
                    Assert.False(memberDoesNotHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldExist.HasViolations(Architecture));
                    Assert.False(membersWithDependencyShouldBeOtherMembers.HasViolations(Architecture));
                    Assert.True(membersWithDependencyShouldHaveDependency.HasViolations(Architecture));
                    Assert.True(membersWithDependencyExist.HasViolations(Architecture));
                }

                var memberHasMethodCallDependencies =
                    Members().That().Are(member).Should().HaveMethodCallDependencies();
                var memberDoesNotHaveMethodCallDependencies =
                    Members().That().Are(member).Should().NotHaveMethodCallDependencies();
                var membersWithMethodCallDependenciesDoNotIncludeMember =
                    Members().That().HaveMethodCallDependencies().Should().NotBe(member);
                var membersWithoutMethodCallDependenciesDoNotIncludeMember =
                    Members().That().DoNotHaveMethodCallDependencies().Should().NotBe(member);

                Assert.Equal(member.HasMethodCallDependencies(),
                    memberHasMethodCallDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasMethodCallDependencies(),
                    memberDoesNotHaveMethodCallDependencies.HasViolations(Architecture));
                Assert.Equal(!member.HasMethodCallDependencies(),
                    membersWithMethodCallDependenciesDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(member.HasMethodCallDependencies(),
                    membersWithoutMethodCallDependenciesDoNotIncludeMember.HasViolations(Architecture));
            }

            var membersWithMethodCallDependenciesHaveMethodCallDependencies = Members().That()
                .HaveMethodCallDependencies().Should().HaveMethodCallDependencies();
            var membersWithMethodCallDependenciesDoNotHaveMethodCallDependencies = Members().That()
                .HaveMethodCallDependencies().Should().NotHaveMethodCallDependencies().AndShould().Exist();
            var membersWithoutMethodCallDependenciesHaveMethodCallDependencies = Members().That()
                .DoNotHaveMethodCallDependencies().Should().HaveMethodCallDependencies().AndShould().Exist();
            var membersWithoutMethodCallDependenciesDoNotHaveMethodCallDependencies = Members().That()
                .DoNotHaveMethodCallDependencies().Should().NotHaveMethodCallDependencies();

            Assert.True(membersWithMethodCallDependenciesHaveMethodCallDependencies.HasViolations(Architecture));
            Assert.False(membersWithMethodCallDependenciesDoNotHaveMethodCallDependencies.HasViolations(Architecture));
            Assert.False(membersWithoutMethodCallDependenciesHaveMethodCallDependencies.HasViolations(Architecture));
            Assert.True(
                membersWithoutMethodCallDependenciesDoNotHaveMethodCallDependencies.HasViolations(Architecture));
        }
    }
}