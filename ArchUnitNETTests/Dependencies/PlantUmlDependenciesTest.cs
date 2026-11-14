using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Domain.PlantUml;
using TestAssembly.Diagram.NoDependencies.Independent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Dependencies
{
    public class PlantUmlDependenciesTest
    {
        [Fact]
        public void DiagramWithNoDependenciesFileBased()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                "plantuml_diagram_" + Guid.NewGuid() + ".puml"
            );
            using (FileStream fileStream = File.Create(path))
            {
                TestDiagram
                    .From(fileStream)
                    .Component("A")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();
            }
            AssertNoViolation(path, "NoDependencies");
        }

        [Fact]
        public void DiagramWithNoDependencies()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                TestDiagram
                    .From(memoryStream)
                    .Component("A")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();

                AssertNoViolation(memoryStream, "NoDependencies");
            }
        }

        [Fact]
        public void DefinedButUnusedDependencyIsAllowed()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                TestDiagram
                    .From(memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("SomeTarget")
                    .WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .DependencyFrom("SomeOrigin")
                    .To("SomeTarget")
                    .Write();

                AssertNoViolation(memoryStream, "NoDependencies");
            }
        }

        [Theory]
        [InlineData(null, "[Origin] --> [Target]")]
        [InlineData(null, "Origin-u->Target")]
        [InlineData(null, "[Origin] -[#red]up-> [Target]")]
        [InlineData(null, "[Target]<-l-[Origin] : uses")]
        [InlineData("#palegreen", "[Origin] --> [Target]")]
        [InlineData("#palegreen", "Origin-u->Target")]
        [InlineData("#palegreen", "[Origin] -[#red]up-> [Target]")]
        [InlineData("#palegreen", "[Target]<-l-[Origin] : uses")]
        [InlineData(null, "[Origin] --|> [Target]")]
        [InlineData(null, "[Target] <|-- [Origin]")]
        [InlineData(null, "[Origin] \"1\" --|> \"many\" [Target]")]
        [InlineData(null, "Origin ..> Target")]
        [InlineData(null, "[Origin] *-- [Target]")]
        [InlineData(null, "[Target] --o [Origin]")]
        public void FormattedDiagramWithDependencyIsAllowed(string color, string dependency)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CreateSimpleDependencyDiagram(memoryStream, color).RawLine(dependency).Write();

                AssertNoViolation(memoryStream, "SimpleDependency");
            }
        }

        [Theory]
        [InlineData("#palegreen")]
        [InlineData("#red;line:blue")]
        public void FormattingDoesNotChangeViolations(string color)
        {
            Architecture architecture = GetArchitectureFrom("SimpleDependency");
            using (MemoryStream plainStream = new MemoryStream())
            using (MemoryStream formattedStream = new MemoryStream())
            {
                CreateSimpleDependencyDiagram(plainStream, null).Write();
                CreateSimpleDependencyDiagram(formattedStream, color).Write();

                List<string> plainViolations = GetViolations(plainStream, architecture);
                List<string> formattedViolations = GetViolations(formattedStream, architecture);

                Assert.NotEmpty(plainViolations);
                Assert.Equal(plainViolations, formattedViolations);
            }
        }

        [Theory]
        [InlineData("[Origin] -- [Target]")]
        [InlineData("[Origin] #-- [Target]")]
        [InlineData("[Origin] <-- [Target]")]
        [InlineData("[Origin] --* [Target]")]
        public void LinksThatDoNotAllowTheDependencyDoNotChangeViolations(string link)
        {
            Architecture architecture = GetArchitectureFrom("SimpleDependency");
            using (MemoryStream withoutLinkStream = new MemoryStream())
            using (MemoryStream withLinkStream = new MemoryStream())
            {
                CreateSimpleDependencyDiagram(withoutLinkStream, null).Write();
                CreateSimpleDependencyDiagram(withLinkStream, null).RawLine(link).Write();

                List<string> violationsWithoutLink = GetViolations(withoutLinkStream, architecture);
                List<string> violationsWithLink = GetViolations(withLinkStream, architecture);

                Assert.NotEmpty(violationsWithoutLink);
                Assert.Equal(violationsWithoutLink, violationsWithLink);
            }
        }

        private static TestDiagram CreateSimpleDependencyDiagram(Stream stream, string color)
        {
            return TestDiagram
                .From(stream)
                .Component("Origin")
                .WithColor(color)
                .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Origin.*")
                .Component("Target")
                .WithColor(color)
                .WithStereoTypes("TestAssembly.Diagram.SimpleDependency.Target.*");
        }

        private static List<string> GetViolations(Stream stream, Architecture architecture)
        {
            return Types()
                .Should()
                .AdhereToPlantUmlDiagram(stream)
                .Evaluate(architecture)
                .Where(result => !result.Passed)
                .Select(result => result.Description)
                .ToList();
        }

        private void AssertNoViolation(Stream stream, string namespc)
        {
            var check = Types().Should().AdhereToPlantUmlDiagram(stream);
            Architecture architecture = GetArchitectureFrom(namespc);
            Assert.True(check.HasNoViolations(architecture));
        }

        private void AssertNoViolation(string file, string namespc)
        {
            var check = Types().Should().AdhereToPlantUmlDiagram(file);
            Architecture architecture = GetArchitectureFrom(namespc);
            Assert.True(check.HasNoViolations(architecture));
        }

        private static Architecture GetArchitectureFrom(string namespc)
        {
            Architecture architecture = new ArchLoader()
                .LoadNamespacesWithinAssembly(
                    typeof(IndependentClass).Assembly,
                    "TestAssembly.Diagram." + namespc
                )
                .Build();
            if (architecture.Classes.Count() == 0)
            {
                throw new InvalidOperationException(
                    string.Format("No classes were imported from '{0}'", namespc)
                );
            }
            return architecture;
        }
    }
}
