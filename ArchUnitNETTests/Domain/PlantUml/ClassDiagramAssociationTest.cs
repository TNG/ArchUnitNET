using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TestAssembly.Diagram.SimpleDependency.Origin;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class ClassDiagramAssociationTest
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        [Fact]
        public void GetNamespaceIdentifierAssociatedWithClass()
        {
            string expectedNamespaceIdentifier = typeof(SomeOriginClass).Namespace;
            ClassDiagramAssociation classDiagramAssociation = CreateAssociation(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithStereoTypes(expectedNamespaceIdentifier)
                    .Component("B").WithStereoTypes(".*.Noclasshere")
                    .Write());

            Class clazz = Architecture.GetClassOfType(typeof(SomeOriginClass));
            Assert.Equal(expectedNamespaceIdentifier, classDiagramAssociation.GetNamespaceIdentifiersFromComponentOf(clazz).Single());
        }

        private ClassDiagramAssociation CreateAssociation(string file)
        {
            PlantUmlDiagram diagram = new PlantUmlParser().Parse(file);
            return new ClassDiagramAssociation(diagram);
        }
    }
}
