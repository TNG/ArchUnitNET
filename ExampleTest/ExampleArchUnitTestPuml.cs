using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ExampleTest.PlantUml.Addresses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ExampleTest
{
    public class ExampleArchUnitTestPuml
    {
        private static readonly Architecture Architecture = new ArchLoader().LoadNamespacesWithinAssembly(typeof(Address).Assembly, "ExampleTest.PlantUml").Build();

        [Fact]
        public void ClassesShouldAdhereToShoppingExampleConsideringOnlyDependenciesInDiagram()
        {
            var filename = "./Resources/shopping_example.puml";

            IArchRule adhereToPlantUmlDiagram = Types().Should().AdhereToPlantUmlDiagram(filename);
            adhereToPlantUmlDiagram.Check(Architecture);
        }
    }
}
