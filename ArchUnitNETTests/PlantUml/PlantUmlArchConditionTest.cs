using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestAssembly.Diagram.NoDependencies.Independent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.PlantUml
{
    public class PlantUmlArchConditionTest
    {
        [Fact]
        public void DiagramWithNoDependencies()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("B").WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .Write();

            AssertNoViolation(file, "NoDependencies");
        }

        [Fact]
        public void DefinedButUnusedDependencyIsAllowed()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("SomeOrigin").WithStereoTypes("TestAssembly.Diagram.NoDependencies.Independent.*")
                    .Component("SomeTarget").WithStereoTypes("TestAssembly.Diagram.NoDependencies.SomeNamespace.*")
                    .DependencyFrom("SomeOrigin").To("SomeTarget")
                    .Write();

            AssertNoViolation(file, "NoDependencies");
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
