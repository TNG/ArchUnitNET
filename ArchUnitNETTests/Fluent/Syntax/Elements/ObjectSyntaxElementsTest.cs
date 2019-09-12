using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class ObjectSyntaxElementsTest
    {
        public ObjectSyntaxElementsTest()
        {
            _types = Architecture.Types;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IType> _types;

        private const string NoTypeName = "NotTheNameOfAnyType_58391351286";

        [Fact]
        public void AreTest()
        {
            foreach (var type in _types)
            {
                var typeIsItself = ArchRuleDefinition.Types().That().Are(type).Should().Be(type);
                var typeIsNotItself = ArchRuleDefinition.Types().That().Are(type).Should().NotBe(type);
                var otherTypesAreNotThisType = ArchRuleDefinition.Types().That().AreNot(type).Should().NotBe(type);
                var otherTypesAreThisType = ArchRuleDefinition.Types().That().AreNot(type).Should().Be(type);
                Assert.True(typeIsItself.Check(Architecture));
                Assert.False(typeIsNotItself.Check(Architecture));
                Assert.True(otherTypesAreNotThisType.Check(Architecture));
                Assert.False(otherTypesAreThisType.Check(Architecture));
            }
        }

        [Fact]
        public void DependOnTest()
        {
            foreach (var type in _types)
            {
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var dependency in type.Dependencies)
                {
                    var typeDependsOnDependency = ArchRuleDefinition.Types().That().Are(type).Should()
                        .DependOn(dependency.Origin.FullName).OrShould()
                        .DependOn(dependency.Target
                            .FullName);
                    var typeDoesNotDependOnDependency = ArchRuleDefinition.Types().That().Are(type).Should()
                        .NotDependOn(dependency.Origin.FullName).AndShould().NotDependOn(dependency.Target.FullName);
                    Assert.True(typeDependsOnDependency.Check(Architecture));
                    Assert.False(typeDoesNotDependOnDependency.Check(Architecture));
                }

                var typesDependOnOwnDependencies = ArchRuleDefinition.Types().That().DependOn(type.FullName).Should()
                    .DependOn(type.FullName);
                var typeDoesNotDependOnFalseDependency =
                    ArchRuleDefinition.Types().That().Are(type).Should().NotDependOn(NoTypeName);
                var typeDependsOnFalseDependency =
                    ArchRuleDefinition.Types().That().Are(type).Should().DependOn(NoTypeName);
                Assert.True(typesDependOnOwnDependencies.Check(Architecture));
                Assert.True(typeDoesNotDependOnFalseDependency.Check(Architecture));
                Assert.False(typeDependsOnFalseDependency.Check(Architecture));
            }

            var noTypeDependsOnFalseDependency =
                ArchRuleDefinition.Types().That().DependOn(NoTypeName).Should().NotExist();
            Assert.True(noTypeDependsOnFalseDependency.Check(Architecture));
        }

        [Fact]
        public void ExistTest()
        {
            foreach (var type in _types)
            {
                var typeExists = ArchRuleDefinition.Types().That().Are(type).Should().Exist();
                var typeDoesNotExist = ArchRuleDefinition.Types().That().Are(type).Should().NotExist();

                Assert.True(typeExists.Check(Architecture));
                Assert.False(typeDoesNotExist.Check(Architecture));
            }

            var typesExist = ArchRuleDefinition.Types().Should().Exist();
            var typesDoNotExist = ArchRuleDefinition.Types().Should().NotExist();

            Assert.True(typesExist.Check(Architecture));
            Assert.False(typesDoNotExist.Check(Architecture));
        }

        [Fact]
        public void HaveFullNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightFullName =
                    ArchRuleDefinition.Types().That().Are(type).Should().HaveFullName(type.FullName);
                var typeDoesNotHaveRightFullName =
                    ArchRuleDefinition.Types().That().Are(type).Should().NotHaveFullName(type.FullName);
                var typeHasFalseFullName =
                    ArchRuleDefinition.Types().That().Are(type).Should().HaveFullName(NoTypeName);
                var typeDoesNotHaveFalseFullName =
                    ArchRuleDefinition.Types().That().Are(type).Should().NotHaveFullName(NoTypeName);
                var typesWithSameFullNameAreEqual =
                    ArchRuleDefinition.Types().That().HaveFullName(type.FullName).Should().Be(type);
                var typesWithDifferentFullNamesAreNotEqual = ArchRuleDefinition.Types().That()
                    .DoNotHaveFullName(type.FullName).Should().NotBe(type);

                Assert.True(typeHasRightFullName.Check(Architecture));
                Assert.False(typeDoesNotHaveRightFullName.Check(Architecture));
                Assert.False(typeHasFalseFullName.Check(Architecture));
                Assert.True(typeDoesNotHaveFalseFullName.Check(Architecture));
                Assert.True(typesWithSameFullNameAreEqual.Check(Architecture));
                Assert.True(typesWithDifferentFullNamesAreNotEqual.Check(Architecture));
            }

            var findNoTypesWithFalseFullName =
                ArchRuleDefinition.Types().That().HaveFullName(NoTypeName).Should().NotExist();
            var findTypesWithRightFullName =
                ArchRuleDefinition.Types().That().DoNotHaveFullName(NoTypeName).Should().Exist();

            Assert.True(findNoTypesWithFalseFullName.Check(Architecture));
            Assert.True(findTypesWithRightFullName.Check(Architecture));
        }

        [Fact]
        public void HaveNameEndingWithTest()
        {
            foreach (var type in _types)
            {
                var name = type.Name;
                for (var i = 0; i <= name.Length; i++)
                {
                    var subString = name.Substring(i);
                    var typeNameEndsWithSubstringOfOwnName = ArchRuleDefinition.Types().That().Are(type).Should()
                        .HaveNameEndingWith(subString);
                    var typeNameDoesNotEndWithSubstringOfOwnName = ArchRuleDefinition.Types().That().Are(type).Should()
                        .NotHaveNameEndingWith(subString);

                    Assert.True(typeNameEndsWithSubstringOfOwnName.Check(Architecture));
                    Assert.False(typeNameDoesNotEndWithSubstringOfOwnName.Check(Architecture));
                }

                var typeNameDoesNotEndWithFalseTypeName = ArchRuleDefinition.Types().That().Are(type).Should()
                    .NotHaveNameEndingWith(NoTypeName);
                var typeNameEndsWithFalseTypeName =
                    ArchRuleDefinition.Types().That().Are(type).Should().HaveNameEndingWith(NoTypeName);

                Assert.True(typeNameDoesNotEndWithFalseTypeName.Check(Architecture));
                Assert.False(typeNameEndsWithFalseTypeName.Check(Architecture));
            }

            var findNoTypesEndingWithFalseName =
                ArchRuleDefinition.Types().That().HaveNameEndingWith(NoTypeName).Or().HaveNameContaining(NoTypeName)
                    .Should().NotExist();
            var findTypesStartingWithRightName =
                ArchRuleDefinition.Types().That().DoNotHaveNameStartingWith(NoTypeName).Or()
                    .DoNotHaveNameContaining(NoTypeName).Should().Exist();

            Assert.True(findNoTypesEndingWithFalseName.Check(Architecture));
            Assert.True(findTypesStartingWithRightName.Check(Architecture));
        }

        [Fact]
        public void HaveNameStartingWithAndHaveNameContainingTest()
        {
            foreach (var type in _types)
            {
                var name = type.Name;
                for (var i = 0; i <= name.Length; i++)
                {
                    for (var j = 1; j <= i; j++)
                    {
                        var subString = name.Substring(j, i - j);
                        var typeNameContainsSubstringOfOwnName = ArchRuleDefinition.Types().That().Are(type).Should()
                            .HaveNameContaining(subString);
                        var typeNameDoesNotContainsSubstringOfOwnName = ArchRuleDefinition.Types().That().Are(type)
                            .Should().NotHaveNameContaining(subString);

                        Assert.True(typeNameContainsSubstringOfOwnName.Check(Architecture));
                        Assert.False(typeNameDoesNotContainsSubstringOfOwnName.Check(Architecture));
                    }

                    var startString = name.Substring(0, i);
                    var typeNameStartsWithAndContainsSubstringOfOwnName = ArchRuleDefinition.Types().That().Are(type)
                        .Should().HaveNameStartingWith(startString).AndShould().HaveNameContaining(startString);
                    var typeNameDoesNotStartWithOrContainSubstringOfOwnName = ArchRuleDefinition.Types().That()
                        .Are(type).Should().NotHaveNameStartingWith(startString).OrShould()
                        .NotHaveNameContaining(startString);

                    Assert.True(typeNameStartsWithAndContainsSubstringOfOwnName.Check(Architecture));
                    Assert.False(typeNameDoesNotStartWithOrContainSubstringOfOwnName.Check(Architecture));
                }

                var typeNameDoesNotStartWithOrContainFalseTypeName = ArchRuleDefinition.Types().That().Are(type)
                    .Should().NotHaveNameStartingWith(NoTypeName).AndShould().NotHaveNameContaining(NoTypeName);
                var typeNameStartsWithOrContainsFalseTypeName = ArchRuleDefinition.Types().That().Are(type)
                    .Should().HaveNameStartingWith(NoTypeName).OrShould().HaveNameContaining(NoTypeName);

                Assert.True(typeNameDoesNotStartWithOrContainFalseTypeName.Check(Architecture));
                Assert.False(typeNameStartsWithOrContainsFalseTypeName.Check(Architecture));
            }

            var findNoTypesStartingWithOrContainingFalseName =
                ArchRuleDefinition.Types().That().HaveNameStartingWith(NoTypeName).Or().HaveNameContaining(NoTypeName)
                    .Should().NotExist();
            var findTypesStartingWithOrContainingRightName =
                ArchRuleDefinition.Types().That().DoNotHaveNameStartingWith(NoTypeName).Or()
                    .DoNotHaveNameContaining(NoTypeName).Should().Exist();

            Assert.True(findNoTypesStartingWithOrContainingFalseName.Check(Architecture));
            Assert.True(findTypesStartingWithOrContainingRightName.Check(Architecture));
        }

        [Fact]
        public void HaveNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightName = ArchRuleDefinition.Types().That().Are(type).Should().HaveName(type.Name);
                var typeDoesNotHaveRightName =
                    ArchRuleDefinition.Types().That().Are(type).Should().NotHaveName(type.Name);
                var typeHasFalseName = ArchRuleDefinition.Types().That().Are(type).Should().HaveName(NoTypeName);
                var typeDoesNotHaveFalseName =
                    ArchRuleDefinition.Types().That().Are(type).Should().NotHaveName(NoTypeName);

                Assert.True(typeHasRightName.Check(Architecture));
                Assert.False(typeDoesNotHaveRightName.Check(Architecture));
                Assert.False(typeHasFalseName.Check(Architecture));
                Assert.True(typeDoesNotHaveFalseName.Check(Architecture));
            }

            var findTypesWithRightName =
                ArchRuleDefinition.Types().That().DoNotHaveName(NoTypeName).Should().Exist();
            var findNoTypesWithFalseName =
                ArchRuleDefinition.Types().That().HaveName(NoTypeName).Should().NotExist();

            Assert.True(findTypesWithRightName.Check(Architecture));
            Assert.True(findNoTypesWithFalseName.Check(Architecture));
        }

        [Fact]
        public void VisibilityTest()
        {
            var visibilityRules = new List<IArchRule>
            {
                ArchRuleDefinition.Types().That().ArePrivate().Should().BePrivate(),
                ArchRuleDefinition.Types().That().ArePublic().Should().BePublic(),
                ArchRuleDefinition.Types().That().AreProtected().Should().BeProtected(),
                ArchRuleDefinition.Types().That().AreInternal().Should().BeInternal(),
                ArchRuleDefinition.Types().That().AreProtectedInternal().Should().BeProtectedInternal(),
                ArchRuleDefinition.Types().That().ArePrivateProtected().Should().BePrivateProtected(),

                ArchRuleDefinition.Types().That().AreNotPrivate().Should().NotBePrivate(),
                ArchRuleDefinition.Types().That().AreNotPublic().Should().NotBePublic(),
                ArchRuleDefinition.Types().That().AreNotProtected().Should().NotBeProtected(),
                ArchRuleDefinition.Types().That().AreNotInternal().Should().NotBeInternal(),
                ArchRuleDefinition.Types().That().AreNotProtectedInternal().Should().NotBeProtectedInternal(),
                ArchRuleDefinition.Types().That().AreNotPrivateProtected().Should().NotBePrivateProtected(),

                ArchRuleDefinition.Types().That().ArePrivate().Should().NotBePublic().AndShould().NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                ArchRuleDefinition.Types().That().ArePublic().Should().NotBePrivate().AndShould().NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                ArchRuleDefinition.Types().That().AreProtected().Should().NotBePublic().AndShould().NotBePrivate()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                ArchRuleDefinition.Types().That().AreInternal().Should().NotBePublic().AndShould().NotBeProtected()
                    .AndShould().NotBePrivate().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                ArchRuleDefinition.Types().That().AreProtectedInternal().Should().NotBePublic().AndShould()
                    .NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBePrivate().AndShould()
                    .NotBePrivateProtected(),
                ArchRuleDefinition.Types().That().ArePrivateProtected().Should().NotBePublic().AndShould()
                    .NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivate(),

                ArchRuleDefinition.Types().That().AreNotPrivate().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                ArchRuleDefinition.Types().That().AreNotPublic().Should().BePrivate().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                ArchRuleDefinition.Types().That().AreNotProtected().Should().BePublic().OrShould().BePrivate()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                ArchRuleDefinition.Types().That().AreNotInternal().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BePrivate().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                ArchRuleDefinition.Types().That().AreNotProtectedInternal().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BePrivate().OrShould()
                    .BePrivateProtected(),
                ArchRuleDefinition.Types().That().AreNotPrivateProtected().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivate(),

                ArchRuleDefinition.Types().That().Are(StaticTestTypes.PublicTestClass).Should().BePublic(),
                ArchRuleDefinition.Types().That().Are(StaticTestTypes.InternalTestClass).Should()
                    .BeInternal(),
                ArchRuleDefinition.Types().That().Are(NestedPrivateTestClass).Should().BePrivate(),
                ArchRuleDefinition.Types().That().Are(NestedPublicTestClass).Should().BePublic(),
                ArchRuleDefinition.Types().That().Are(NestedProtectedTestClass).Should().BeProtected(),
                ArchRuleDefinition.Types().That().Are(NestedInternalTestClass).Should().BeInternal(),
                ArchRuleDefinition.Types().That().Are(NestedProtectedInternalTestClass).Should().BeProtectedInternal(),
                ArchRuleDefinition.Types().That().Are(NestedPrivateProtectedTestClass).Should().BePrivateProtected()
            };

            foreach (var visibilityRule in visibilityRules)
            {
                Assert.True(visibilityRule.Check(Architecture));
            }
        }
    }
}