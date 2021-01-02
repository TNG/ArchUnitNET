using ArchUnitNET.PlantUml;
using ArchUnitNETTests.PlantUml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ArchUnitNETTests.PlantUml
{
    public class PlantUmlParserTest
    {
        private static readonly PlantUmlParser parser = new PlantUmlParser();

        [Fact]
        public void ParsesCorrectNumberOfComponents()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
             .Component("SomeOrigin").WithStereoTypes("..origin..")
             .Component("SomeTarget").WithStereoTypes("..target..")
             .Write());

            Assert.Equal(2, diagram.AllComponents.Count);
        }

        [Fact]
        public void ParsesASimpleComponent()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("SomeOrigin").WithStereoTypes("..origin..")
                    .Write());

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(origin.Stereotypes.SingleOrDefault(), new Stereotype("..origin.."));
            Assert.Null(origin.Alias);
        }

        [Fact]
        public void DoesNotIncludeCommentedOutLines()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("uncommentedComponent").WithAlias("uncommentedAlias").WithStereoTypes("..uncommentedPackage..")
                    .RawLine("  '  [commentedComponent] <<..commentedPackage..>> as commentedAlias")
                    .RawLine("")
                    .RawLine(" ' [uncommentedComponent] --> [commentedComponent]")
                    .Write());

            PlantUmlComponent uncommentedComponent = GetComponentWithName("uncommentedComponent", diagram);

            Assert.Equal(diagram.AllComponents.Single(), uncommentedComponent);
            Assert.Empty(uncommentedComponent.Dependencies);
        }

        [Fact]
        public void DoesNotIncludeDependencyDescriptions()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("component").WithStereoTypes("..somePackage..")
                    .Component("otherComponent").WithStereoTypes("..somePackage2..")
                    .RawLine("[component] --> [otherComponent] : this part should be ignored, no matter the comment tick ' ")
                    .Write());

            PlantUmlComponent component = GetComponentWithName("component", diagram);
            PlantUmlComponent targetOfDescribedDependency = component.Dependencies.Single();
            Assert.Equal(targetOfDescribedDependency.ComponentName, new ComponentName("otherComponent"));
        }

        [Fact]
        public void ThrowsExceptionWithComponentsThatAreNotYetDefined()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .DependencyFrom("[NotYetDefined]").To("[AlsoNotYetDefined]")
                    .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(file));
            Assert.Contains("There is no Component with name or alias = 'NotYetDefined'", exception.Message);
            Assert.Contains("Components must be specified separately from dependencies", exception.Message);
        }

        [Fact]
        public void ThrowsExceptionWithComponentsWithoutStereotypes()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .RawLine("[componentWithoutStereotype]")
                    .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(file));
            Assert.Contains("componentWithoutStereotype", exception.Message);
            Assert.Contains("at least one stereotype specifying the package identifier(<<..>>)", exception.Message);
        }

        [Fact]
        public void ParsesTwoIdenticalComponentsNoDependency()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("someName").WithAlias("someAlias").WithStereoTypes("someStereotype")
                    .Component("someName").WithAlias("someAlias").WithStereoTypes("someStereotype")
                    .Write());

            Assert.Equal(new[] { GetComponentWithName("someName", diagram) }, diagram.AllComponents);
        }

        [Fact]
        public void RejectsAComponentWithAnIllegalAlias()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("irrelevant").WithAlias("ill[]egal").WithStereoTypes("..irrelevant..")
                    .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(file));
            Assert.Contains("Alias 'ill[]egal' should not contain character(s): '[' or ']' or '\"'", exception.Message);
        }

        [Fact]
        public void ParsesATrickyAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("tricky").WithAlias("because it's quoted").WithStereoTypes("..tricky..")
                    .Component("tricky as hell cause of as keyword").WithAlias("other").WithStereoTypes("..other..")
                    .Write());

            PlantUmlComponent trickyAsHell = GetComponentWithName("tricky as hell cause of as keyword", diagram);
            PlantUmlComponent tricky = GetComponentWithName("tricky", diagram);

            Assert.Equal(new Alias("other"), trickyAsHell.Alias);
            Assert.Equal(new Alias("because it's quoted"), tricky.Alias);
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypes()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("someComponent")
                    .WithStereoTypes("..firstPackage..", "..secondPackage..", "..thirdPackage..")
                    .Write());

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(new[] { new Stereotype("..firstPackage.."), new Stereotype("..secondPackage.."), new Stereotype("..thirdPackage..") }, component.Stereotypes.OrderBy(st => st.AsString()));
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypesAndAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("someComponent").WithAlias("someAlias")
                    .WithStereoTypes("..firstPackage..", "..secondPackage..", "..thirdPackage..")
                    .Write());

            PlantUmlComponent component = diagram.AllComponents.Single();

            Assert.Equal(new Alias("someAlias"), component.Alias);
        }

        [Fact]
        public void ParsesDiagramWithDependenciesThatUseAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithAlias("aliasForA").WithStereoTypes("..controller..")
                    .Component("B").WithAlias("aliasForB").WithStereoTypes("..service..")
                    .DependencyFrom("aliasForA").To("aliasForB")
                    .Write());

            PlantUmlComponent aliasForA = GetComponentWithAlias(new Alias("aliasForA"), diagram);
            PlantUmlComponent aliasForB = GetComponentWithAlias(new Alias("aliasForB"), diagram);

            Assert.Equal(aliasForA, GetComponentWithName("A", diagram));
            Assert.Equal(aliasForB, GetComponentWithName("B", diagram));
            Assert.Equal(new[] { aliasForB }, aliasForA.Dependencies);
        }

        [Fact]
        public void ParsesDependenciesBetweenComponentsWithoutBrackets()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithStereoTypes("..origin..")
                    .Component("B").WithStereoTypes("..target..")
                    .DependencyFrom("A").To("B")
                    .Write());

            PlantUmlComponent a = GetComponentWithName("A", diagram);
            PlantUmlComponent b = GetComponentWithName("B", diagram);

            Assert.Equal(new[] { b }, a.Dependencies);
        }

        [Fact]
        public void ParsesMultipleComponentsAndDependencies()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("Component1").WithStereoTypes("..origin1..")
                    .Component("Component2").WithStereoTypes("..target1..")
                    .Component("Component3").WithStereoTypes("..origin2..")
                    .Component("Component4").WithStereoTypes("..target2..")
                    .DependencyFrom("Component1").To("Component2")
                    .DependencyFrom("Component3").To("Component4")
                    .Write();

            PlantUmlDiagram diagram = CreateDiagram(file);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);
            PlantUmlComponent component4 = GetComponentWithName("Component4", diagram);

            Assert.Equal(new[] { component1, component2, component3, component4 }, diagram.AllComponents.OrderBy(c => c.ComponentName.AsString()));
            Assert.Equal(new[] { component2 }, component1.Dependencies);
            Assert.Empty(component2.Dependencies);
            Assert.Equal(new[] { component4 }, component3.Dependencies);
            Assert.Empty(component4.Dependencies);
        }

        [Fact]
        public void ParsesADiagramWithNoUniqueOrigins()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("Component1").WithStereoTypes("..origin..")
                    .Component("Component2").WithStereoTypes("..target1..")
                    .Component("Component3").WithStereoTypes("..target2..")
                    .DependencyFrom("[Component1]").To("[Component2]")
                    .DependencyFrom("[Component1]").To("[Component3]")
                    .Write();

            PlantUmlDiagram diagram = CreateDiagram(file);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component2, component3 }, component1.Dependencies);
        }

        [Fact]
        public void ParseADiagramWithNonUniqueTargets()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("Component1").WithStereoTypes("..origin1..")
                    .Component("Component2").WithStereoTypes("..origin2..")
                    .Component("Component3").WithStereoTypes("..target..")
                    .DependencyFrom("[Component1]").To("[Component3]")
                    .DependencyFrom("[Component2]").To("[Component3]")
                    .Write();

            PlantUmlDiagram diagram = CreateDiagram(file);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component3 }, component1.Dependencies);
            Assert.Equal(new[] { component3 }, component2.Dependencies);
        }

        [Fact]
        public void ParseAComponentDiagramWithBothAliasAndNamesUsed()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithAlias("foo").WithStereoTypes("..service..")
                    .Component("B").WithStereoTypes("..controller..")
                    .DependencyFrom("[B]").To("foo")
                    .DependencyFrom("foo").To("[B]")
                    .Write());

            PlantUmlComponent componentB = GetComponentWithName("B", diagram);
            PlantUmlComponent componentFoo = GetComponentWithAlias(new Alias("foo"), diagram);

            Assert.Equal(new[] { componentFoo }, componentB.Dependencies);
            Assert.Equal(new[] { componentB }, componentFoo.Dependencies);
        }

        [Fact]
        public void ParsesAComponentDiagramThatUsesAliasWithAndWithoutBrackets()
        {
            string file = TestDiagram.In(Path.GetTempPath())
                    .Component("A").WithAlias("foo").WithStereoTypes("..origin..")
                    .Component("B").WithAlias("bar").WithStereoTypes("..target..")
                    .DependencyFrom("foo").To("bar")
                    .DependencyFrom("[foo]").To("[bar]")
                    .Write();

            PlantUmlDiagram diagram = CreateDiagram(file);

            PlantUmlComponent foo = GetComponentWithAlias(new Alias("foo"), diagram);
            PlantUmlComponent bar = GetComponentWithAlias(new Alias("bar"), diagram);

            Assert.Equal(new[] { bar }, foo.Dependencies);
            Assert.Empty(bar.Dependencies);
        }


        private PlantUmlComponent GetComponentWithName(string componentName, PlantUmlDiagram diagram)
        {
            PlantUmlComponent component = diagram.AllComponents
                .Where(comp => Equals(comp.ComponentName, new ComponentName(componentName)))
                .First();
            return component;
        }

        private PlantUmlComponent GetComponentWithAlias(Alias alias, PlantUmlDiagram diagram)
        {
            return diagram.ComponentsWithAlias.Where(c => alias.Equals(c.Alias)).Single();
        }

        private PlantUmlDiagram CreateDiagram(string path)
        {
            return parser.Parse(path);
        }
    }
}
