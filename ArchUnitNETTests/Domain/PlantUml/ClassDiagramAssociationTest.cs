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
using TestAssembly;
using TestAssembly.Diagram.ConfusingNamespaceNames.FooNamespace.BarNamespace;

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

        [Fact]
        public void GetTargetNamespaceIdentifierOfClass()
        {
            string expectedTarget1 = ".*.Target1";
            string expectedTarget2 = ".*.Target2";
            
            ClassDiagramAssociation classDiagramAssociation = CreateAssociation(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithStereoTypes(Regex.Replace(typeof(SomeOriginClass).Namespace, @".*\.", ".*."))
                    .Component("B").WithStereoTypes(expectedTarget1)
                    .Component("C").WithStereoTypes(expectedTarget2)
                    .DependencyFrom("[A]").To("[B]")
                    .DependencyFrom("[A]").To("[C]")
                    .Write());

            Class clazz = Architecture.GetClassOfType(typeof(SomeOriginClass));

            Assert.Equal(new[] { expectedTarget1, expectedTarget2 }, classDiagramAssociation.GetTargetNamespaceIdentifiers(clazz).OrderBy(s => s).ToList());
        }

        [Fact]
        public void RejectsClassNotContainedInAnyComponent()
        {
            ClassDiagramAssociation classDiagramAssociation = CreateAssociation(TestDiagram.In(Path.GetTempPath())
                    .Component("SomeComponent").WithStereoTypes(".*.SomeStereotype.")
                    .Write());

            Class classNotContained = Architecture.GetClassOfType(typeof(object));

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => classDiagramAssociation.GetTargetNamespaceIdentifiers(classNotContained));
            Assert.Equal(string.Format("Class {0} is not contained in any component", typeof(object).Name), exception.Message);
        }

        [Fact]
        public void ReportsIfClassIsContainedInAnyComponent()
        {
            ClassDiagramAssociation classDiagramAssociation = CreateAssociation(TestDiagram.In(Path.GetTempPath())
                    .Component("Object").WithStereoTypes(typeof(object).Namespace)
                    .Write());

            Assert.True(classDiagramAssociation.Contains(Architecture.GetClassOfType(typeof(object))), "association contains " + typeof(object).Name);
            Assert.False(classDiagramAssociation.Contains(Architecture.GetClassOfType(typeof(Class2))), "association contains " + typeof(Class2).Name);
        }

        [Fact]
        public void ClassResidesInMultipleNamespaces()
        {
            ClassDiagramAssociation classDiagramAssociation = CreateAssociation(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithStereoTypes(".*.FooNamespace.*")
                    .Component("B").WithStereoTypes(".*.BarNamespace.*")
                    .Write());

            Class classContainedInTwoComponents = Architecture.GetClassOfType(typeof(ClassInFooAndBarNamespace));

            ComponentIntersectionException exception = Assert.Throws<ComponentIntersectionException>(() => classDiagramAssociation.GetTargetNamespaceIdentifiers(classContainedInTwoComponents));
            Assert.Equal(string.Format("Class {0} may not be contained in more than one component, but is contained in [A, B]", typeof(ClassInFooAndBarNamespace).Name), exception.Message);
        }

        [Fact]
        public void RejectsDuplicateStereotype()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("first").WithStereoTypes(".*.Identical.*")
                    .Component("second").WithStereoTypes(".*.Identical.*")
                    .Write();

            Class classContainedInTwoComponents = Architecture.GetClassOfType(typeof(ClassInFooAndBarNamespace));

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateAssociation(file));
            Assert.Equal("Stereotype '.*.Identical.*' should be unique", exception.Message);
        }

        private ClassDiagramAssociation CreateAssociation(string file)
        {
            PlantUmlDiagram diagram = new PlantUmlParser().Parse(file);
            return new ClassDiagramAssociation(diagram);
        }
    }
}
