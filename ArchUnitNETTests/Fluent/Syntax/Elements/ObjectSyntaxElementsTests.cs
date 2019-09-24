using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNETTests.Domain.StaticTestTypes;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class ObjectSyntaxElementsTests
    {
        public ObjectSyntaxElementsTests()
        {
            _types = Architecture.Types;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IType> _types;
        private const string NoTypeName = "NotTheNameOfAnyType_58391351286";

        [Fact]
        public void AreTest()
        {
            //Tests with one argument

            foreach (var type in _types)
            {
                var typeIsItself = Types().That().Are(type).Should().Be(type);
                var typeIsNotItself = Types().That().Are(type).Should().NotBe(type);
                var otherTypesAreNotThisType = Types().That().AreNot(type).Should().NotBe(type);
                var otherTypesAreThisType = Types().That().AreNot(type).Should().Be(type);

                Assert.True(typeIsItself.Check(Architecture));
                Assert.False(typeIsNotItself.Check(Architecture));
                Assert.True(otherTypesAreNotThisType.Check(Architecture));
                Assert.False(otherTypesAreThisType.Check(Architecture));
            }

            var publicTestClassIsPublic = Types().That().Are(StaticTestTypes.PublicTestClass).Should().BePublic();
            var publicTestClassIsNotPublic = Types().That().Are(StaticTestTypes.PublicTestClass).Should().NotBePublic();
            var notPublicTypesAreNotPublicTestClass =
                Types().That().AreNotPublic().Should().NotBe(StaticTestTypes.PublicTestClass);
            var publicTypesAreNotPublicTestClass =
                Types().That().ArePublic().Should().NotBe(StaticTestTypes.PublicTestClass);

            Assert.True(publicTestClassIsPublic.Check(Architecture));
            Assert.False(publicTestClassIsNotPublic.Check(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClass.Check(Architecture));
            Assert.False(publicTypesAreNotPublicTestClass.Check(Architecture));


            //Tests with multiple arguments

            var publicTestClassAndInternalTestClassIsPublicOrInternal = Types().That()
                .Are(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass).Should().BePublic().OrShould()
                .BeInternal();
            var publicTestClassAndInternalTestClassIsPublic = Types().That()
                .Are(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass).Should().BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types().That().AreNotPublic()
                .And().AreNotInternal().Should()
                .NotBe(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass);
            var internalTypesAreNotPublicTestClassOrInternalTestClass = Types().That().AreInternal().Should()
                .NotBe(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass);

            Assert.True(publicTestClassAndInternalTestClassIsPublicOrInternal.Check(Architecture));
            Assert.False(publicTestClassAndInternalTestClassIsPublic.Check(Architecture));
            Assert.True(notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.Check(Architecture));
            Assert.False(internalTypesAreNotPublicTestClassOrInternalTestClass.Check(Architecture));
        }

        [Fact]
        public void DependOnTest()
        {
            foreach (var type in _types)
            {
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var dependency in type.Dependencies)
                {
                    var typeDependsOnDependency = Types().That().Are(type).Should()
                        .DependOn(dependency.Origin.FullName).OrShould()
                        .DependOn(dependency.Target
                            .FullName);
                    var typeDoesNotDependOnDependency = Types().That().Are(type).Should()
                        .NotDependOn(dependency.Origin.FullName).AndShould().NotDependOn(dependency.Target.FullName);

                    Assert.True(typeDependsOnDependency.Check(Architecture));
                    Assert.False(typeDoesNotDependOnDependency.Check(Architecture));
                }

                var typesDependOnOwnDependencies = Types().That().DependOn(type.FullName).Should()
                    .DependOn(type.FullName);
                var typeDoesNotDependOnFalseDependency =
                    Types().That().Are(type).Should().NotDependOn(NoTypeName);
                var typeDependsOnFalseDependency =
                    Types().That().Are(type).Should().DependOn(NoTypeName);

                Assert.True(typesDependOnOwnDependencies.Check(Architecture));
                Assert.True(typeDoesNotDependOnFalseDependency.Check(Architecture));
                Assert.False(typeDependsOnFalseDependency.Check(Architecture));
            }

            var noTypeDependsOnFalseDependency =
                Types().That().DependOn(NoTypeName).Should().NotExist();

            Assert.True(noTypeDependsOnFalseDependency.Check(Architecture));
        }

        [Fact]
        public void ExistTest()
        {
            foreach (var type in _types)
            {
                var typeExists = Types().That().Are(type).Should().Exist();
                var typeDoesNotExist = Types().That().Are(type).Should().NotExist();

                Assert.True(typeExists.Check(Architecture));
                Assert.False(typeDoesNotExist.Check(Architecture));
            }

            var typesExist = Types().Should().Exist();
            var typesDoNotExist = Types().Should().NotExist();

            Assert.True(typesExist.Check(Architecture));
            Assert.False(typesDoNotExist.Check(Architecture));
        }

        [Fact]
        public void HaveFullNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightFullName =
                    Types().That().Are(type).Should().HaveFullName(type.FullName);
                var typeDoesNotHaveRightFullName =
                    Types().That().Are(type).Should().NotHaveFullName(type.FullName);
                var typeHasFalseFullName =
                    Types().That().Are(type).Should().HaveFullName(NoTypeName);
                var typeDoesNotHaveFalseFullName =
                    Types().That().Are(type).Should().NotHaveFullName(NoTypeName);
                var typesWithSameFullNameAreEqual =
                    Types().That().HaveFullName(type.FullName).Should().Be(type);
                var typesWithDifferentFullNamesAreNotEqual = Types().That()
                    .DoNotHaveFullName(type.FullName).Should().NotBe(type);

                Assert.True(typeHasRightFullName.Check(Architecture));
                Assert.False(typeDoesNotHaveRightFullName.Check(Architecture));
                Assert.False(typeHasFalseFullName.Check(Architecture));
                Assert.True(typeDoesNotHaveFalseFullName.Check(Architecture));
                Assert.True(typesWithSameFullNameAreEqual.Check(Architecture));
                Assert.True(typesWithDifferentFullNamesAreNotEqual.Check(Architecture));
            }

            var findNoTypesWithFalseFullName =
                Types().That().HaveFullName(NoTypeName).Should().NotExist();
            var findTypesWithRightFullName =
                Types().That().DoNotHaveFullName(NoTypeName).Should().Exist();

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
                    var typeNameEndsWithSubstringOfOwnName = Types().That().Are(type).Should()
                        .HaveNameEndingWith(subString);
                    var typeNameDoesNotEndWithSubstringOfOwnName = Types().That().Are(type).Should()
                        .NotHaveNameEndingWith(subString);

                    Assert.True(typeNameEndsWithSubstringOfOwnName.Check(Architecture));
                    Assert.False(typeNameDoesNotEndWithSubstringOfOwnName.Check(Architecture));
                }

                var typeNameDoesNotEndWithFalseTypeName = Types().That().Are(type).Should()
                    .NotHaveNameEndingWith(NoTypeName);
                var typeNameEndsWithFalseTypeName =
                    Types().That().Are(type).Should().HaveNameEndingWith(NoTypeName);

                Assert.True(typeNameDoesNotEndWithFalseTypeName.Check(Architecture));
                Assert.False(typeNameEndsWithFalseTypeName.Check(Architecture));
            }

            var findNoTypesEndingWithFalseName =
                Types().That().HaveNameEndingWith(NoTypeName).Or().HaveNameContaining(NoTypeName)
                    .Should().NotExist();
            var findTypesStartingWithRightName =
                Types().That().DoNotHaveNameStartingWith(NoTypeName).Or()
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
                        var typeNameContainsSubstringOfOwnName = Types().That().Are(type).Should()
                            .HaveNameContaining(subString);
                        var typeNameDoesNotContainsSubstringOfOwnName = Types().That().Are(type)
                            .Should().NotHaveNameContaining(subString);

                        Assert.True(typeNameContainsSubstringOfOwnName.Check(Architecture));
                        Assert.False(typeNameDoesNotContainsSubstringOfOwnName.Check(Architecture));
                    }

                    var startString = name.Substring(0, i);
                    var typeNameStartsWithAndContainsSubstringOfOwnName = Types().That().Are(type)
                        .Should().HaveNameStartingWith(startString).AndShould().HaveNameContaining(startString);
                    var typeNameDoesNotStartWithOrContainSubstringOfOwnName = Types().That()
                        .Are(type).Should().NotHaveNameStartingWith(startString).OrShould()
                        .NotHaveNameContaining(startString);

                    Assert.True(typeNameStartsWithAndContainsSubstringOfOwnName.Check(Architecture));
                    Assert.False(typeNameDoesNotStartWithOrContainSubstringOfOwnName.Check(Architecture));
                }

                var typeNameDoesNotStartWithOrContainFalseTypeName = Types().That().Are(type)
                    .Should().NotHaveNameStartingWith(NoTypeName).AndShould().NotHaveNameContaining(NoTypeName);
                var typeNameStartsWithOrContainsFalseTypeName = Types().That().Are(type)
                    .Should().HaveNameStartingWith(NoTypeName).OrShould().HaveNameContaining(NoTypeName);

                Assert.True(typeNameDoesNotStartWithOrContainFalseTypeName.Check(Architecture));
                Assert.False(typeNameStartsWithOrContainsFalseTypeName.Check(Architecture));
            }

            var findNoTypesStartingWithOrContainingFalseName =
                Types().That().HaveNameStartingWith(NoTypeName).Or().HaveNameContaining(NoTypeName)
                    .Should().NotExist();
            var findTypesStartingWithOrContainingRightName =
                Types().That().DoNotHaveNameStartingWith(NoTypeName).Or()
                    .DoNotHaveNameContaining(NoTypeName).Should().Exist();

            Assert.True(findNoTypesStartingWithOrContainingFalseName.Check(Architecture));
            Assert.True(findTypesStartingWithOrContainingRightName.Check(Architecture));
        }

        [Fact]
        public void HaveNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightName = Types().That().Are(type).Should().HaveName(type.Name);
                var typeDoesNotHaveRightName =
                    Types().That().Are(type).Should().NotHaveName(type.Name);
                var typeHasFalseName = Types().That().Are(type).Should().HaveName(NoTypeName);
                var typeDoesNotHaveFalseName =
                    Types().That().Are(type).Should().NotHaveName(NoTypeName);

                Assert.True(typeHasRightName.Check(Architecture));
                Assert.False(typeDoesNotHaveRightName.Check(Architecture));
                Assert.False(typeHasFalseName.Check(Architecture));
                Assert.True(typeDoesNotHaveFalseName.Check(Architecture));
            }

            var findTypesWithRightName =
                Types().That().DoNotHaveName(NoTypeName).Should().Exist();
            var findNoTypesWithFalseName =
                Types().That().HaveName(NoTypeName).Should().NotExist();

            Assert.True(findTypesWithRightName.Check(Architecture));
            Assert.True(findNoTypesWithFalseName.Check(Architecture));
        }

        [Fact]
        public void VisibilityTest()
        {
            var visibilityRules = new List<IArchRule>
            {
                Types().That().ArePrivate().Should().BePrivate(),
                Types().That().ArePublic().Should().BePublic(),
                Types().That().AreProtected().Should().BeProtected(),
                Types().That().AreInternal().Should().BeInternal(),
                Types().That().AreProtectedInternal().Should().BeProtectedInternal(),
                Types().That().ArePrivateProtected().Should().BePrivateProtected(),

                Types().That().AreNotPrivate().Should().NotBePrivate(),
                Types().That().AreNotPublic().Should().NotBePublic(),
                Types().That().AreNotProtected().Should().NotBeProtected(),
                Types().That().AreNotInternal().Should().NotBeInternal(),
                Types().That().AreNotProtectedInternal().Should().NotBeProtectedInternal(),
                Types().That().AreNotPrivateProtected().Should().NotBePrivateProtected(),

                Types().That().ArePrivate().Should().NotBePublic().AndShould().NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                Types().That().ArePublic().Should().NotBePrivate().AndShould().NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                Types().That().AreProtected().Should().NotBePublic().AndShould().NotBePrivate()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                Types().That().AreInternal().Should().NotBePublic().AndShould().NotBeProtected()
                    .AndShould().NotBePrivate().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivateProtected(),
                Types().That().AreProtectedInternal().Should().NotBePublic().AndShould()
                    .NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBePrivate().AndShould()
                    .NotBePrivateProtected(),
                Types().That().ArePrivateProtected().Should().NotBePublic().AndShould()
                    .NotBeProtected()
                    .AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould()
                    .NotBePrivate(),

                Types().That().AreNotPrivate().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                Types().That().AreNotPublic().Should().BePrivate().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                Types().That().AreNotProtected().Should().BePublic().OrShould().BePrivate()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                Types().That().AreNotInternal().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BePrivate().OrShould().BeProtectedInternal().OrShould()
                    .BePrivateProtected(),
                Types().That().AreNotProtectedInternal().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BePrivate().OrShould()
                    .BePrivateProtected(),
                Types().That().AreNotPrivateProtected().Should().BePublic().OrShould().BeProtected()
                    .OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould()
                    .BePrivate(),

                Types().That().Are(StaticTestTypes.PublicTestClass).Should().BePublic(),
                Types().That().Are(StaticTestTypes.InternalTestClass).Should()
                    .BeInternal(),
                Types().That().Are(NestedPrivateTestClass).Should().BePrivate(),
                Types().That().Are(NestedPublicTestClass).Should().BePublic(),
                Types().That().Are(NestedProtectedTestClass).Should().BeProtected(),
                Types().That().Are(NestedInternalTestClass).Should().BeInternal(),
                Types().That().Are(NestedProtectedInternalTestClass).Should().BeProtectedInternal(),
                Types().That().Are(NestedPrivateProtectedTestClass).Should().BePrivateProtected()
            };

            foreach (var visibilityRule in visibilityRules)
            {
                Assert.True(visibilityRule.Check(Architecture));
            }
        }
    }
}