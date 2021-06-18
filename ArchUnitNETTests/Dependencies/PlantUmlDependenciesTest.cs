using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Domain.PlantUml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            string path = Path.Combine(Path.GetTempPath(), "plantuml_diagram_" + Guid.NewGuid() + ".puml");
            using (FileStream fileStream = File.Create(path))
            {
                TestDiagram.From(fileStream)
                    .Component("A").WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B").WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();
            }
            AssertNoViolation(path, "NoDependencies");
        }

        [Fact]
        public void DiagramWithNoDependencies()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                TestDiagram.From(memoryStream)
                    .Component("A").WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B").WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();

                AssertNoViolation(memoryStream, "NoDependencies");
            }
        }

        [Fact]
        public void DefinedButUnusedDependencyIsAllowed()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                TestDiagram.From(memoryStream)
                    .Component("SomeOrigin").WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("SomeTarget").WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .DependencyFrom("SomeOrigin").To("SomeTarget")
                    .Write();

                AssertNoViolation(memoryStream, "NoDependencies");
            }
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
            Architecture architecture = new ArchLoader().LoadNamespacesWithinAssembly(typeof(IndependentClass).Assembly, "TestAssembly.Diagram." + namespc).Build();
            if (architecture.Classes.Count() == 0)
            {
                throw new InvalidOperationException(
                        string.Format("No classes were imported from '{0}'", namespc));
            }
            return architecture;
        }
    }


}
