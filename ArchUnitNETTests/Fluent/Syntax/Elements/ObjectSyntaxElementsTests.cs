using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class ObjectSyntaxElementsTests
    {
        private const string NoTypeName = "NotTheNameOfAnyType_58391351286";
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly IEnumerable<Type> _falseDependencies = new List<Type>
        {
            typeof(ClassWithNoDependencies1),
            typeof(ClassWithNoDependencies2),
        };

        private readonly IEnumerable<IType> _types = Architecture.Types;

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

                var typeIsItselfPattern = Types()
                    .That()
                    .Are(type.FullName)
                    .Should()
                    .Be(type.FullName);
                var typeIsNotItselfPattern = Types()
                    .That()
                    .Are(type.FullName)
                    .Should()
                    .NotBe(type.FullName);
                var otherTypesAreNotThisTypePattern = Types()
                    .That()
                    .AreNot(type.FullName)
                    .Should()
                    .NotBe(type.FullName);
                var otherTypesAreThisTypePattern = Types()
                    .That()
                    .AreNot(type.FullName)
                    .Should()
                    .Be(type.FullName);

                Assert.True(typeIsItself.HasNoViolations(Architecture));
                Assert.False(typeIsNotItself.HasNoViolations(Architecture));
                Assert.True(otherTypesAreNotThisType.HasNoViolations(Architecture));
                Assert.False(otherTypesAreThisType.HasNoViolations(Architecture));

                Assert.True(typeIsItselfPattern.HasNoViolations(Architecture));
                Assert.False(typeIsNotItselfPattern.HasNoViolations(Architecture));
                Assert.True(otherTypesAreNotThisTypePattern.HasNoViolations(Architecture));
                Assert.False(otherTypesAreThisTypePattern.HasNoViolations(Architecture));
            }

            var publicTestClassIsPublic = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass)
                .Should()
                .BePublic();
            var publicTestClassIsNotPublic = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass)
                .Should()
                .NotBePublic();
            var notPublicTypesAreNotPublicTestClass = Types()
                .That()
                .AreNotPublic()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass);
            var publicTypesAreNotPublicTestClass = Types()
                .That()
                .ArePublic()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass);

            var publicTestClassIsPublicPattern = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass.FullName)
                .Should()
                .BePublic();
            var publicTestClassIsNotPublicPattern = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass.FullName)
                .Should()
                .NotBePublic();
            var notPublicTypesAreNotPublicTestClassPattern = Types()
                .That()
                .AreNotPublic()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass.FullName);
            var publicTypesAreNotPublicTestClassPattern = Types()
                .That()
                .ArePublic()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass.FullName);

            Assert.True(publicTestClassIsPublic.HasNoViolations(Architecture));
            Assert.False(publicTestClassIsNotPublic.HasNoViolations(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClass.HasNoViolations(Architecture));
            Assert.False(publicTypesAreNotPublicTestClass.HasNoViolations(Architecture));

            Assert.True(publicTestClassIsPublicPattern.HasNoViolations(Architecture));
            Assert.False(publicTestClassIsNotPublicPattern.HasNoViolations(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClassPattern.HasNoViolations(Architecture));
            Assert.False(publicTypesAreNotPublicTestClassPattern.HasNoViolations(Architecture));

            //Tests with multiple arguments


            var publicTestClassAndInternalTestClassIsPublicOrInternal = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass)
                .Should()
                .BePublic()
                .OrShould()
                .BeInternal();
            var publicTestClassAndInternalTestClassIsPublic = Types()
                .That()
                .Are(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass)
                .Should()
                .BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types()
                .That()
                .AreNotPublic()
                .And()
                .AreNotInternal()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass);
            var internalTypesAreNotPublicTestClassOrInternalTestClass = Types()
                .That()
                .AreInternal()
                .Should()
                .NotBe(StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass);

            Assert.True(
                publicTestClassAndInternalTestClassIsPublicOrInternal.HasNoViolations(Architecture)
            );
            Assert.False(publicTestClassAndInternalTestClassIsPublic.HasNoViolations(Architecture));
            Assert.True(
                notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(
                    Architecture
                )
            );
            Assert.False(
                internalTypesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(Architecture)
            );

            //Tests with list

            var list = new List<Class>
            {
                StaticTestTypes.PublicTestClass,
                StaticTestTypes.InternalTestClass,
            };
            var listPublicTestClassAndInternalTestClassIsPublicOrInternal = Types()
                .That()
                .Are(list)
                .Should()
                .BePublic()
                .OrShould()
                .BeInternal();
            var listPublicTestClassAndInternalTestClassIsPublic = Types()
                .That()
                .Are(list)
                .Should()
                .BePublic();
            var listNotPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types()
                .That()
                .AreNotPublic()
                .And()
                .AreNotInternal()
                .Should()
                .NotBe(list);
            var listInternalTypesAreNotPublicTestClassOrInternalTestClass = Types()
                .That()
                .AreInternal()
                .Should()
                .NotBe(list);

            var patternList = new List<string>
            {
                StaticTestTypes.PublicTestClass.FullName,
                StaticTestTypes.InternalTestClass.FullName,
            };
            var publicTestClassAndInternalTestClassIsPublicOrInternalPattern = Types()
                .That()
                .Are(patternList)
                .Should()
                .BePublic()
                .OrShould()
                .BeInternal();
            var publicTestClassAndInternalTestClassIsPublicPattern = Types()
                .That()
                .Are(patternList)
                .Should()
                .BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClassPattern =
                Types().That().AreNotPublic().And().AreNotInternal().Should().NotBe(patternList);
            var internalTypesAreNotPublicTestClassOrInternalTestClassPattern = Types()
                .That()
                .AreInternal()
                .Should()
                .NotBe(patternList);

            Assert.True(
                listPublicTestClassAndInternalTestClassIsPublicOrInternal.HasNoViolations(
                    Architecture
                )
            );
            Assert.False(
                listPublicTestClassAndInternalTestClassIsPublic.HasNoViolations(Architecture)
            );
            Assert.True(
                listNotPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(
                    Architecture
                )
            );
            Assert.False(
                listInternalTypesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(
                    Architecture
                )
            );

            Assert.True(
                publicTestClassAndInternalTestClassIsPublicOrInternalPattern.HasNoViolations(
                    Architecture
                )
            );
            Assert.False(
                publicTestClassAndInternalTestClassIsPublicPattern.HasNoViolations(Architecture)
            );
            Assert.True(
                notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClassPattern.HasNoViolations(
                    Architecture
                )
            );
            Assert.False(
                internalTypesAreNotPublicTestClassOrInternalTestClassPattern.HasNoViolations(
                    Architecture
                )
            );
        }

        [Fact]
        public void DependOnPatternTest()
        {
            foreach (var type in _types)
            {
                //One Argument

                var typesDependOnOwnDependencies = Types()
                    .That()
                    .DependOnAny(type.FullName)
                    .Should()
                    .DependOnAny(type.FullName)
                    .WithoutRequiringPositiveResults();
                var typeDoesNotDependOnFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAny(NoTypeName);
                var typeDependsOnFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .DependOnAny(NoTypeName)
                    .WithoutRequiringPositiveResults();

                Assert.True(typesDependOnOwnDependencies.HasNoViolations(Architecture));
                Assert.True(typeDoesNotDependOnFalseDependency.HasNoViolations(Architecture));
                Assert.False(typeDependsOnFalseDependency.HasNoViolations(Architecture));

                //Multiple Arguments

                var patternList = new List<string> { type.FullName, NoTypeName };
                var typesDependOnOwnDependenciesMultiple = Types()
                    .That()
                    .DependOnAny(patternList)
                    .Should()
                    .DependOnAny(patternList)
                    .WithoutRequiringPositiveResults();
                var typeDependsOnFalseDependencyMultiple = Types()
                    .That()
                    .Are(patternList)
                    .Should()
                    .DependOnAny(NoTypeName);

                Assert.True(typesDependOnOwnDependenciesMultiple.HasNoViolations(Architecture));
                Assert.False(typeDependsOnFalseDependencyMultiple.HasNoViolations(Architecture));
            }

            var noTypeDependsOnFalseDependency = Types()
                .That()
                .DependOnAny(NoTypeName)
                .Should()
                .NotExist();
            var typesDoNotDependsOnFalseDependency = Types()
                .That()
                .DoNotDependOnAny(NoTypeName)
                .Should()
                .Exist();

            Assert.True(noTypeDependsOnFalseDependency.HasNoViolations(Architecture));
            Assert.True(typesDoNotDependsOnFalseDependency.HasNoViolations(Architecture));
        }

        [Fact]
        public void DependOnTypesTest()
        {
            foreach (var type in _types)
            {
                if (type.FullNameContains("ObjectSyntaxElementsTests"))
                {
                    continue;
                }

                //One Argument
                var typeDependencies = type.GetTypeDependencies(Architecture).ToList();
                var typesDependOnOwnDependencies = Types()
                    .That()
                    .DependOnAny(type)
                    .Should()
                    .DependOnAnyTypesThat()
                    .Are(type)
                    .WithoutRequiringPositiveResults();
                var typeDoesNotDependOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAnyTypesThat()
                    .Are(typeof(ClassWithNoDependencies1));
                var typeDependsOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .DependOnAnyTypesThat()
                    .Are(typeof(ClassWithNoDependencies1));
                var typeOnlyDependsOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOnTypesThat()
                    .Are(typeof(ClassWithNoDependencies1));

                Assert.True(typesDependOnOwnDependencies.HasNoViolations(Architecture));
                typeDoesNotDependOnOneFalseDependency.Check(Architecture);
                Assert.True(typeDoesNotDependOnOneFalseDependency.HasNoViolations(Architecture));
                Assert.False(typeDependsOnOneFalseDependency.HasNoViolations(Architecture));
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnOneFalseDependency.HasNoViolations(Architecture)
                );

                //Multiple Arguments

                var typeDoesNotDependOnMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAnyTypesThat()
                    .Are(typeof(ClassWithNoDependencies1), typeof(ClassWithNoDependencies2));
                var typeOnlyDependsOnMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOnTypesThat()
                    .Are(typeof(ClassWithNoDependencies1), typeof(ClassWithNoDependencies2));

                Assert.True(
                    typeDoesNotDependOnMultipleFalseDependencies.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnMultipleFalseDependencies.HasNoViolations(Architecture)
                );

                //Multiple Arguments as IEnumerable


                var typeOnlyDependsOnOwnDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOnTypesThat()
                    .Are(typeDependencies);
                var typeDoesNotDependsOnOwnDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAnyTypesThat()
                    .Are(typeDependencies);
                var typeDoesNotDependOnListOfMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAnyTypesThat()
                    .Are(_falseDependencies);
                var typeOnlyDependsOnListOfMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOnTypesThat()
                    .Are(_falseDependencies);

                Assert.True(typeOnlyDependsOnOwnDependencies.HasNoViolations(Architecture));
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeDoesNotDependsOnOwnDependencies.HasNoViolations(Architecture)
                );
                Assert.True(
                    typeDoesNotDependOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
                );
            }
        }

        [Fact]
        public void DependOnTypesThatTest()
        {
            foreach (var type in _types)
            {
                if (type.FullNameContains("ObjectSyntaxElementsTests"))
                {
                    continue;
                }

                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var dependency in type.Dependencies)
                {
                    var typeDependsOnDependency = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .DependOnAny(dependency.Target);
                    var typeDoesNotDependOnDependency = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .NotDependOnAny(dependency.Target);

                    Assert.True(typeDependsOnDependency.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotDependOnDependency.HasNoViolations(Architecture));
                }

                var typeDependencies = type.GetTypeDependencies(Architecture).ToList();

                //One Argument

                var typesDependOnOwnDependencies = Types()
                    .That()
                    .DependOnAny(type)
                    .Should()
                    .DependOnAny(type)
                    .WithoutRequiringPositiveResults();
                var typeDoesNotDependOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAny(typeof(ClassWithNoDependencies1));
                var typeDependsOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .DependOnAny(typeof(ClassWithNoDependencies1));
                var typeOnlyDependsOnOneFalseDependency = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOn(typeof(ClassWithNoDependencies1));

                Assert.True(typesDependOnOwnDependencies.HasNoViolations(Architecture));
                Assert.True(typeDoesNotDependOnOneFalseDependency.HasNoViolations(Architecture));
                Assert.False(typeDependsOnOneFalseDependency.HasNoViolations(Architecture));
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnOneFalseDependency.HasNoViolations(Architecture)
                );

                //Multiple Arguments

                var typeDoesNotDependOnMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAny(
                        typeof(ClassWithNoDependencies1),
                        typeof(ClassWithNoDependencies2)
                    );
                var typeOnlyDependsOnMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOn(
                        typeof(ClassWithNoDependencies1),
                        typeof(ClassWithNoDependencies2)
                    );

                Assert.True(
                    typeDoesNotDependOnMultipleFalseDependencies.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnMultipleFalseDependencies.HasNoViolations(Architecture)
                );

                //Multiple Arguments as IEnumerable

                var typeOnlyDependsOnOwnDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOn(typeDependencies);
                var typeDoesNotDependsOnOwnDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAny(typeDependencies);
                var typeDoesNotDependOnListOfMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotDependOnAny(_falseDependencies);
                var typeOnlyDependsOnListOfMultipleFalseDependencies = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .OnlyDependOn(_falseDependencies);

                Assert.True(typeOnlyDependsOnOwnDependencies.HasNoViolations(Architecture));
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeDoesNotDependsOnOwnDependencies.HasNoViolations(Architecture)
                );
                Assert.True(
                    typeDoesNotDependOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    typeDependencies.IsNullOrEmpty(),
                    typeOnlyDependsOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
                );
            }

            var noTypeDependsOnFalseDependency = Types()
                .That()
                .DependOnAny(typeof(ClassWithNoDependencies1))
                .Should()
                .Be(typeof(ObjectSyntaxElementsTests));
            var noTypeDependsOnMultipleFalseDependencies = Types()
                .That()
                .DependOnAny(typeof(ClassWithNoDependencies1), typeof(ClassWithNoDependencies2))
                .Should()
                .Be(typeof(ObjectSyntaxElementsTests));
            var noTypeDependsOnListOfMultipleFalseDependencies = Types()
                .That()
                .DependOnAny(_falseDependencies)
                .Should()
                .Be(typeof(ObjectSyntaxElementsTests));
            var typesDoNotDependOnFalseDependency = Types()
                .That()
                .DoNotDependOnAny(typeof(ClassWithNoDependencies1))
                .Should()
                .Exist();
            var typesDoNotDependOnMultipleFalseDependencies = Types()
                .That()
                .DoNotDependOnAny(
                    typeof(ClassWithNoDependencies1),
                    typeof(ClassWithNoDependencies2)
                )
                .Should()
                .Exist();
            var typeDoNotDependOnListOfMultipleFalseDependencies = Types()
                .That()
                .DoNotDependOnAny(_falseDependencies)
                .Should()
                .Exist();

            Assert.True(noTypeDependsOnFalseDependency.HasNoViolations(Architecture));
            Assert.True(noTypeDependsOnMultipleFalseDependencies.HasNoViolations(Architecture));
            Assert.True(
                noTypeDependsOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
            );
            Assert.True(typesDoNotDependOnFalseDependency.HasNoViolations(Architecture));
            Assert.True(typesDoNotDependOnMultipleFalseDependencies.HasNoViolations(Architecture));
            Assert.True(
                typeDoNotDependOnListOfMultipleFalseDependencies.HasNoViolations(Architecture)
            );

            //Fluent arguments

            var typeThatDependOnTypesWithPDoNotDependOnTypesWithP = Types()
                .That()
                .DependOnAny(Types().That().HaveNameStartingWith("P"))
                .Should()
                .NotDependOnAny(Types().That().HaveNameStartingWith("P"));
            var typesThatDependOnFalseTypeShouldDependOnNoType = Types()
                .That()
                .OnlyDependOn(Classes().That().Are(typeof(ClassWithNoDependencies1)))
                .Should()
                .NotDependOnAny(Types());

            Assert.False(
                typeThatDependOnTypesWithPDoNotDependOnTypesWithP.HasNoViolations(Architecture)
            );
            Assert.True(
                typesThatDependOnFalseTypeShouldDependOnNoType.HasNoViolations(Architecture)
            );
        }

        [Fact]
        public void ExistTest()
        {
            foreach (var type in _types)
            {
                var typeExists = Types().That().Are(type).Should().Exist();
                var typeDoesNotExist = Types().That().Are(type).Should().NotExist();

                Assert.True(typeExists.HasNoViolations(Architecture));
                Assert.False(typeDoesNotExist.HasNoViolations(Architecture));
            }

            var typesExist = Types().Should().Exist();
            var typesDoNotExist = Types().Should().NotExist();

            Assert.True(typesExist.HasNoViolations(Architecture));
            Assert.False(typesDoNotExist.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveFullNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightFullName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .HaveFullName(type.FullName);
                var typeDoesNotHaveRightFullName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveFullName(type.FullName);
                var typeHasFalseFullName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .HaveFullName(NoTypeName);
                var typeDoesNotHaveFalseFullName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveFullName(NoTypeName);
                var typesWithSameFullNameAreEqual = Types()
                    .That()
                    .HaveFullName(type.FullName)
                    .Should()
                    .Be(type);
                var typesWithDifferentFullNamesAreNotEqual = Types()
                    .That()
                    .DoNotHaveFullName(type.FullName)
                    .Should()
                    .NotBe(type);

                Assert.True(typeHasRightFullName.HasNoViolations(Architecture));
                Assert.False(typeDoesNotHaveRightFullName.HasNoViolations(Architecture));
                Assert.False(typeHasFalseFullName.HasNoViolations(Architecture));
                Assert.True(typeDoesNotHaveFalseFullName.HasNoViolations(Architecture));
                Assert.True(typesWithSameFullNameAreEqual.HasNoViolations(Architecture));
                Assert.True(typesWithDifferentFullNamesAreNotEqual.HasNoViolations(Architecture));
            }

            var findNoTypesWithFalseFullName = Types()
                .That()
                .HaveFullName(NoTypeName)
                .Should()
                .NotExist();
            var findTypesWithRightFullName = Types()
                .That()
                .DoNotHaveFullName(NoTypeName)
                .Should()
                .Exist();

            Assert.True(findNoTypesWithFalseFullName.HasNoViolations(Architecture));
            Assert.True(findTypesWithRightFullName.HasNoViolations(Architecture));
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
                    var typeNameEndsWithSubstringOfOwnName = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .HaveNameEndingWith(subString);
                    var typeNameDoesNotEndWithSubstringOfOwnName = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .NotHaveNameEndingWith(subString);

                    Assert.True(typeNameEndsWithSubstringOfOwnName.HasNoViolations(Architecture));
                    Assert.False(
                        typeNameDoesNotEndWithSubstringOfOwnName.HasNoViolations(Architecture)
                    );
                }

                var typeNameDoesNotEndWithFalseTypeName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveNameEndingWith(NoTypeName);
                var typeNameEndsWithFalseTypeName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .HaveNameEndingWith(NoTypeName);

                Assert.True(typeNameDoesNotEndWithFalseTypeName.HasNoViolations(Architecture));
                Assert.False(typeNameEndsWithFalseTypeName.HasNoViolations(Architecture));
            }

            var findNoTypesEndingWithFalseName = Types()
                .That()
                .HaveNameEndingWith(NoTypeName)
                .Or()
                .HaveNameContaining(NoTypeName)
                .Should()
                .NotExist();
            var findTypesStartingWithRightName = Types()
                .That()
                .DoNotHaveNameStartingWith(NoTypeName)
                .Or()
                .DoNotHaveNameContaining(NoTypeName)
                .Should()
                .Exist();

            Assert.True(findNoTypesEndingWithFalseName.HasNoViolations(Architecture));
            Assert.True(findTypesStartingWithRightName.HasNoViolations(Architecture));
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
                        var typeNameContainsSubstringOfOwnName = Types()
                            .That()
                            .Are(type)
                            .Should()
                            .HaveNameContaining(subString);
                        var typeNameDoesNotContainsSubstringOfOwnName = Types()
                            .That()
                            .Are(type)
                            .Should()
                            .NotHaveNameContaining(subString);

                        Assert.True(
                            typeNameContainsSubstringOfOwnName.HasNoViolations(Architecture)
                        );
                        Assert.False(
                            typeNameDoesNotContainsSubstringOfOwnName.HasNoViolations(Architecture)
                        );
                    }

                    var startString = name.Substring(0, i);
                    var typeNameStartsWithAndContainsSubstringOfOwnName = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .HaveNameStartingWith(startString)
                        .AndShould()
                        .HaveNameContaining(startString);
                    var typeNameDoesNotStartWithOrContainSubstringOfOwnName = Types()
                        .That()
                        .Are(type)
                        .Should()
                        .NotHaveNameStartingWith(startString)
                        .OrShould()
                        .NotHaveNameContaining(startString);

                    Assert.True(
                        typeNameStartsWithAndContainsSubstringOfOwnName.HasNoViolations(
                            Architecture
                        )
                    );
                    Assert.False(
                        typeNameDoesNotStartWithOrContainSubstringOfOwnName.HasNoViolations(
                            Architecture
                        )
                    );
                }

                var typeNameDoesNotStartWithOrContainFalseTypeName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveNameStartingWith(NoTypeName)
                    .AndShould()
                    .NotHaveNameContaining(NoTypeName);
                var typeNameStartsWithOrContainsFalseTypeName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .HaveNameStartingWith(NoTypeName)
                    .OrShould()
                    .HaveNameContaining(NoTypeName);

                Assert.True(
                    typeNameDoesNotStartWithOrContainFalseTypeName.HasNoViolations(Architecture)
                );
                Assert.False(
                    typeNameStartsWithOrContainsFalseTypeName.HasNoViolations(Architecture)
                );
            }

            var findNoTypesStartingWithOrContainingFalseName = Types()
                .That()
                .HaveNameStartingWith(NoTypeName)
                .Or()
                .HaveNameContaining(NoTypeName)
                .Should()
                .NotExist();
            var findTypesStartingWithOrContainingRightName = Types()
                .That()
                .DoNotHaveNameStartingWith(NoTypeName)
                .Or()
                .DoNotHaveNameContaining(NoTypeName)
                .Should()
                .Exist();

            Assert.True(findNoTypesStartingWithOrContainingFalseName.HasNoViolations(Architecture));
            Assert.True(findTypesStartingWithOrContainingRightName.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveNameTest()
        {
            foreach (var type in _types)
            {
                var typeHasRightName = Types().That().Are(type).Should().HaveName(type.Name);
                var typeDoesNotHaveRightName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveName(type.Name);
                var typeHasFalseName = Types().That().Are(type).Should().HaveName(NoTypeName);
                var typeDoesNotHaveFalseName = Types()
                    .That()
                    .Are(type)
                    .Should()
                    .NotHaveName(NoTypeName);

                Assert.True(typeHasRightName.HasNoViolations(Architecture));
                Assert.False(typeDoesNotHaveRightName.HasNoViolations(Architecture));
                Assert.False(typeHasFalseName.HasNoViolations(Architecture));
                Assert.True(typeDoesNotHaveFalseName.HasNoViolations(Architecture));
            }

            var findTypesWithRightName = Types().That().DoNotHaveName(NoTypeName).Should().Exist();
            var findNoTypesWithFalseName = Types().That().HaveName(NoTypeName).Should().NotExist();

            Assert.True(findTypesWithRightName.HasNoViolations(Architecture));
            Assert.True(findNoTypesWithFalseName.HasNoViolations(Architecture));
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
                Types()
                    .That()
                    .ArePrivate()
                    .Should()
                    .NotBePublic()
                    .AndShould()
                    .NotBeProtected()
                    .AndShould()
                    .NotBeInternal()
                    .AndShould()
                    .NotBeProtectedInternal()
                    .AndShould()
                    .NotBePrivateProtected(),
                Types()
                    .That()
                    .ArePublic()
                    .Should()
                    .NotBePrivate()
                    .AndShould()
                    .NotBeProtected()
                    .AndShould()
                    .NotBeInternal()
                    .AndShould()
                    .NotBeProtectedInternal()
                    .AndShould()
                    .NotBePrivateProtected(),
                Types()
                    .That()
                    .AreProtected()
                    .Should()
                    .NotBePublic()
                    .AndShould()
                    .NotBePrivate()
                    .AndShould()
                    .NotBeInternal()
                    .AndShould()
                    .NotBeProtectedInternal()
                    .AndShould()
                    .NotBePrivateProtected(),
                Types()
                    .That()
                    .AreInternal()
                    .Should()
                    .NotBePublic()
                    .AndShould()
                    .NotBeProtected()
                    .AndShould()
                    .NotBePrivate()
                    .AndShould()
                    .NotBeProtectedInternal()
                    .AndShould()
                    .NotBePrivateProtected(),
                Types()
                    .That()
                    .AreProtectedInternal()
                    .Should()
                    .NotBePublic()
                    .AndShould()
                    .NotBeProtected()
                    .AndShould()
                    .NotBeInternal()
                    .AndShould()
                    .NotBePrivate()
                    .AndShould()
                    .NotBePrivateProtected(),
                Types()
                    .That()
                    .ArePrivateProtected()
                    .Should()
                    .NotBePublic()
                    .AndShould()
                    .NotBeProtected()
                    .AndShould()
                    .NotBeInternal()
                    .AndShould()
                    .NotBeProtectedInternal()
                    .AndShould()
                    .NotBePrivate(),
                Types()
                    .That()
                    .AreNotPrivate()
                    .Should()
                    .BePublic()
                    .OrShould()
                    .BeProtected()
                    .OrShould()
                    .BeInternal()
                    .OrShould()
                    .BeProtectedInternal()
                    .OrShould()
                    .BePrivateProtected(),
                Types()
                    .That()
                    .AreNotPublic()
                    .Should()
                    .BePrivate()
                    .OrShould()
                    .BeProtected()
                    .OrShould()
                    .BeInternal()
                    .OrShould()
                    .BeProtectedInternal()
                    .OrShould()
                    .BePrivateProtected(),
                Types()
                    .That()
                    .AreNotProtected()
                    .Should()
                    .BePublic()
                    .OrShould()
                    .BePrivate()
                    .OrShould()
                    .BeInternal()
                    .OrShould()
                    .BeProtectedInternal()
                    .OrShould()
                    .BePrivateProtected(),
                Types()
                    .That()
                    .AreNotInternal()
                    .Should()
                    .BePublic()
                    .OrShould()
                    .BeProtected()
                    .OrShould()
                    .BePrivate()
                    .OrShould()
                    .BeProtectedInternal()
                    .OrShould()
                    .BePrivateProtected(),
                Types()
                    .That()
                    .AreNotProtectedInternal()
                    .Should()
                    .BePublic()
                    .OrShould()
                    .BeProtected()
                    .OrShould()
                    .BeInternal()
                    .OrShould()
                    .BePrivate()
                    .OrShould()
                    .BePrivateProtected(),
                Types()
                    .That()
                    .AreNotPrivateProtected()
                    .Should()
                    .BePublic()
                    .OrShould()
                    .BeProtected()
                    .OrShould()
                    .BeInternal()
                    .OrShould()
                    .BeProtectedInternal()
                    .OrShould()
                    .BePrivate(),
                Types().That().Are(StaticTestTypes.PublicTestClass).Should().BePublic(),
                Types().That().Are(StaticTestTypes.InternalTestClass).Should().BeInternal(),
                Types().That().Are(NestedPrivateTestClass).Should().BePrivate(),
                Types().That().Are(NestedPublicTestClass).Should().BePublic(),
                Types().That().Are(NestedProtectedTestClass).Should().BeProtected(),
                Types().That().Are(NestedInternalTestClass).Should().BeInternal(),
                Types().That().Are(NestedProtectedInternalTestClass).Should().BeProtectedInternal(),
                Types().That().Are(NestedPrivateProtectedTestClass).Should().BePrivateProtected(),
            };

            foreach (var visibilityRule in visibilityRules)
            {
                Assert.True(visibilityRule.HasNoViolations(Architecture));
            }
        }
    }

    public class ClassWithNoDependencies1 { }

    public class ClassWithNoDependencies2 { }

    public interface IInterfaceWithNoDependencies1 { }

    public interface IInterfaceWithNoDependencies2 { }
}
