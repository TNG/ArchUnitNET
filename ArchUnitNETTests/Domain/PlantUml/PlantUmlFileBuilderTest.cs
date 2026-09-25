using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using ArchUnitNET.Domain.PlantUml.Export;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using VerifyXunit;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class PlantUmlFileBuilderTest
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(PlantUmlFileBuilderTest).Assembly)
            .Build();

        private static readonly List<IPlantUmlElement> Dependencies = new List<IPlantUmlElement>
        {
            new PlantUmlDependency("a", "b", DependencyType.OneToOne),
            new PlantUmlDependency("b", "c", DependencyType.OneToOne),
            new PlantUmlDependency("c", "a", DependencyType.OneToOne),
        };

        [Fact]
        public void BuildUmlByTypesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(
                Architecture.Types.Take(100)
            );
            var uml = builder.AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByTypesIncludingDependenciesToOtherTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(
                Architecture.Types.Take(100),
                new GenerationOptions { IncludeDependenciesToOther = true }
            );
            var uml = builder.AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByNamespacesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Namespaces);
            var uml = builder.AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlBySlicesTest()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching("ArchUnitNETTests.(*).")
                .GetObjects(Architecture);
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(slices);
            var uml = builder.AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public Task BuildUmlByDependenciesTest()
        {
            return VerifyElements(Dependencies);
        }

        [Fact]
        public Task BuildUmlByDependenciesWithObjectsWithNoDependenciesTest()
        {
            var classesWithoutDependencies = new[] { new PlantUmlClass("d") };
            return VerifyElements(Dependencies.Concat(classesWithoutDependencies));
        }

        [Fact]
        public void SliceWithHyperlinkAppendsHyperlinkTest()
        {
            var slice = new PlantUmlSlice("Slice1", hyperlink: "https://example.com");
            var uml = slice.GetPlantUmlString(new RenderOptions());
            Assert.Equal("[Slice1] [[https://example.com]] " + Environment.NewLine, uml);
        }

        [Fact]
        public void SliceWithHyperlinkAndColorPutsHyperlinkBeforeColorTest()
        {
            var slice = new PlantUmlSlice(
                "Slice1",
                color: "99ffd1",
                hyperlink: "https://example.com"
            );
            var uml = slice.GetPlantUmlString(new RenderOptions());
            Assert.Equal("[Slice1] [[https://example.com]]  #99ffd1" + Environment.NewLine, uml);
        }

        [Fact]
        public Task NestedSliceWithHyperlinkTest()
        {
            return VerifyElements(
                new[] { new PlantUmlSlice("A.B.C", "A.", hyperlink: "https://example.com") }
            );
        }

        [Fact]
        public Task SlicesSharingParentPackagesTest()
        {
            return VerifyElements(
                new[]
                {
                    new PlantUmlSlice("A.X.One", "A."),
                    new PlantUmlSlice("A.Y.Two", "A."),
                    new PlantUmlSlice("A.X.Deep.Three", "A."),
                    new PlantUmlSlice("B.Four", "B."),
                }
            );
        }

        [Fact]
        public Task PackageOnlySliceWithColorTest()
        {
            return VerifyElements(
                new[]
                {
                    new PlantUmlSlice("A.X.One", "A."),
                    new PlantUmlSlice("A.X.", "A.", "99ffd1"),
                }
            );
        }

        [Fact]
        public Task FlatSlicesNextToNestedSlicesTest()
        {
            return VerifyElements(
                new[]
                {
                    new PlantUmlSlice("Flat1"),
                    new PlantUmlSlice("A.X", "A."),
                    new PlantUmlSlice("Flat2"),
                    new PlantUmlSlice("A.Y", "A."),
                }
            );
        }

        [Fact]
        public Task C4StyleSlicesSharingParentBoundariesTest()
        {
            var one = new PlantUmlSlice("A.X.One", "A.");
            var two = new PlantUmlSlice("A.X.Two", "A.");
            one.UseS4Style();
            two.UseS4Style();
            return VerifyElements(new[] { one, two });
        }

        [Fact]
        public void HandleIllegalComponentNamesTest()
        {
            var forbiddenCharacters = new[] { "[", "]", "\r", "\n", "\f", "\a", "\b", "\v" };
            foreach (var character in forbiddenCharacters)
            {
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlDependency(character, "a", DependencyType.OneToOne)
                );
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlDependency("a", character, DependencyType.OneToOne)
                );
                Assert.Throws<IllegalComponentNameException>(() => new PlantUmlClass(character));
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlInterface(character)
                );
                Assert.Throws<IllegalComponentNameException>(() => new PlantUmlSlice(character));
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlNamespace(character)
                );
            }
        }

        [Fact]
        public Task SpecialCharactersInComponentNamesTest()
        {
            var dependenciesWithSpecialCharacters = new List<IPlantUmlElement>
            {
                new PlantUmlDependency("!\"§´`", "$%&/()=?", DependencyType.OneToOne),
                new PlantUmlDependency("\\\t%", "äöüß", DependencyType.OneToOne),
                new PlantUmlDependency("^°-*+.,;:", "<>|@€", DependencyType.OneToOne),
            };
            var classesWithSpecialCharacters = new[]
            {
                new PlantUmlClass("!§´`$%&/()=?\\\täöüß^°-*+,-.,;:<>|@€"),
            };
            return VerifyElements(
                dependenciesWithSpecialCharacters.Concat(classesWithSpecialCharacters)
            );
        }

        private static Task VerifyElements(IEnumerable<IPlantUmlElement> elements)
        {
            return Verifier
                .Verify(new PlantUmlFileBuilder().WithElements(elements).AsString())
                .DisableDiff() // Don't open diff tool during the test
                .UseDirectory("Snapshots");
        }
    }

    internal class ClassToFocusOn { }

    internal class DependantClassOfFocusedClass { }

    internal class DependingClassOfFocusedClass { }
}
