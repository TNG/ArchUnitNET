using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain.PlantUml;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class PlantUmlParserTest : IDisposable
    {
        private readonly PlantUmlParser _parser = new PlantUmlParser();
        private readonly MemoryStream _memoryStream;

        public PlantUmlParserTest()
        {
            _parser = new PlantUmlParser();
            _memoryStream = new MemoryStream();
        }

        public void Dispose()
        {
            _memoryStream.Close();
        }

        [Fact]
        public void ParsesCorrectNumberOfComponents()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("SomeOrigin").WithStereoTypes("Origin.*")
                .Component("SomeTarget").WithStereoTypes("Target.*")
                .Write());

            Assert.Equal(2, diagram.AllComponents.Count);
        }

        [Fact]
        public void ParsesASimpleComponent()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                   .Component("SomeOrigin").WithStereoTypes("Origin.*")
                   .Write());

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(origin.Stereotypes.SingleOrDefault(), new Stereotype("Origin.*"));
            Assert.Null(origin.Alias);
        }

        [Theory]
        [ClassData(typeof(SimpleDiagramTestData))]
        public void ParsesDependencyOfSimpleComponentDiagram(Func<TestDiagram, TestDiagram> testCaseFunc)
        {
            TestDiagram initialDiagram = TestDiagram.From(_memoryStream)
                .Component("SomeOrigin").WithStereoTypes("Origin.*")
                .Component("SomeTarget").WithStereoTypes("Target.*");
            PlantUmlDiagram diagram = CreateDiagram(testCaseFunc(initialDiagram).Write());

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            PlantUmlComponent target = origin.Dependencies.Single();

            Assert.Equal(target.ComponentName, new ComponentName("SomeTarget"));
            Assert.Empty(target.Dependencies);
            Assert.Equal(new Stereotype("Target.*"), target.Stereotypes.Single());
            Assert.Null(target.Alias);
        }

        [Theory]
        [ClassData(typeof(DependencyArrowTestData))]
        public void ParsesVariousTypesOfDependencyArrows(string dependency)
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("SomeOrigin").WithStereoTypes("Origin.*")
                .Component("SomeTarget").WithStereoTypes("Target.*")
                .RawLine(dependency)
                .Write());

            PlantUmlComponent component = GetComponentWithName("SomeOrigin", diagram);
            PlantUmlComponent target = component.Dependencies.Single();

            Assert.Equal(target.ComponentName, new ComponentName("SomeTarget"));
        }

        [Fact]
        public void DoesNotIncludeCommentedOutLines()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("uncommentedComponent").WithAlias("uncommentedAlias").WithStereoTypes("UncommentedNamespace.*")
                .RawLine("  '  [commentedComponent] <<CommentedNamespace.*>> as commentedAlias")
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
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("component").WithStereoTypes("SomeNamespace.*")
                .Component("otherComponent").WithStereoTypes("SomeNamespace2.*")
                .RawLine("[component] --> [otherComponent] : this part should be ignored, no matter the comment tick ' ")
                .Write());

            PlantUmlComponent component = GetComponentWithName("component", diagram);
            PlantUmlComponent targetOfDescribedDependency = component.Dependencies.Single();
            Assert.Equal(targetOfDescribedDependency.ComponentName, new ComponentName("otherComponent"));
        }

        [Fact]
        public void ThrowsExceptionWithComponentsThatAreNotYetDefined()
        {
            TestDiagram.From(_memoryStream)
                .DependencyFrom("[NotYetDefined]").To("[AlsoNotYetDefined]")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(_memoryStream));
            Assert.Contains("There is no Component with name or alias = 'NotYetDefined'", exception.Message);
            Assert.Contains("Components must be specified separately from dependencies", exception.Message);
        }

        [Fact]
        public void ThrowsExceptionWithComponentsWithoutStereotypes()
        {
            TestDiagram.From(_memoryStream)
                .RawLine("[componentWithoutStereotype]")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(_memoryStream));
            Assert.Contains("componentWithoutStereotype", exception.Message);
            Assert.Contains("at least one stereotype specifying the namespace identifier (<<.*>>)", exception.Message);
        }

        [Fact]
        public void ParsesTwoIdenticalComponentsNoDependency()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("someName").WithAlias("someAlias").WithStereoTypes("someStereotype")
                .Component("someName").WithAlias("someAlias").WithStereoTypes("someStereotype")
                .Write());

            Assert.Equal(new[] { GetComponentWithName("someName", diagram) }, diagram.AllComponents);
        }

        [Fact]
        public void RejectsAComponentWithAnIllegalAlias()
        {
            TestDiagram.From(_memoryStream)
                .Component("irrelevant").WithAlias("ill[]egal").WithStereoTypes("Irrelevant.*")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() => CreateDiagram(_memoryStream));
            Assert.Contains("Alias 'ill[]egal' should not contain character(s): '[' or ']' or '\"'", exception.Message);
        }

        [Fact]
        public void ParsesATrickyAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("tricky").WithAlias("because it's quoted").WithStereoTypes("Tricky.*")
                .Component("tricky as hell cause of as keyword").WithAlias("other").WithStereoTypes("Other.*")
                .Write());

            PlantUmlComponent trickyAsHell = GetComponentWithName("tricky as hell cause of as keyword", diagram);
            PlantUmlComponent tricky = GetComponentWithName("tricky", diagram);

            Assert.Equal(new Alias("other"), trickyAsHell.Alias);
            Assert.Equal(new Alias("because it's quoted"), tricky.Alias);
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypes()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("someComponent")
                .WithStereoTypes("FirstNamespace.*", "SecondNamespace.*", "ThirdNamespace.*")
                .Write());

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(new[] { new Stereotype("FirstNamespace.*"), new Stereotype("SecondNamespace.*"), new Stereotype("ThirdNamespace.*") },
                component.Stereotypes.OrderBy(st => st.AsString()));
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypesAndAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("someComponent").WithAlias("someAlias")
                .WithStereoTypes("FirstNamespace.*", "SecondNamespace.*", "ThirdNamespace.*")
                .Write());

            PlantUmlComponent component = diagram.AllComponents.Single();

            Assert.Equal(new Alias("someAlias"), component.Alias);
        }

        [Fact]
        public void ParsesDiagramWithDependenciesThatUseAlias()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("A").WithAlias("aliasForA").WithStereoTypes("Controller.*")
                .Component("B").WithAlias("aliasForB").WithStereoTypes("Service.*")
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
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("A").WithStereoTypes("Origin.*")
                .Component("B").WithStereoTypes("Target.*")
                .DependencyFrom("A").To("B")
                .Write());

            PlantUmlComponent a = GetComponentWithName("A", diagram);
            PlantUmlComponent b = GetComponentWithName("B", diagram);

            Assert.Equal(new[] { b }, a.Dependencies);
        }

        [Fact]
        public void ParsesMultipleComponentsAndDependencies()
        {
            TestDiagram.From(_memoryStream)
                .Component("Component1").WithStereoTypes("Origin1.*")
                .Component("Component2").WithStereoTypes("Target1.*")
                .Component("Component3").WithStereoTypes("Origin2.*")
                .Component("Component4").WithStereoTypes("Target2.*")
                .DependencyFrom("Component1").To("Component2")
                .DependencyFrom("Component3").To("Component4")
                .Write();

            PlantUmlDiagram diagram = CreateDiagram(_memoryStream);

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
            TestDiagram.From(_memoryStream)
                .Component("Component1").WithStereoTypes("Origin.*")
                .Component("Component2").WithStereoTypes("Target1.*")
                .Component("Component3").WithStereoTypes("Target2.*")
                .DependencyFrom("[Component1]").To("[Component2]")
                .DependencyFrom("[Component1]").To("[Component3]")
                .Write();

            PlantUmlDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component2, component3 }, component1.Dependencies);
        }

        [Fact]
        public void ParseADiagramWithNonUniqueTargets()
        {
            TestDiagram.From(_memoryStream)
                .Component("Component1").WithStereoTypes("Origin1.*")
                .Component("Component2").WithStereoTypes("Origin2.*")
                .Component("Component3").WithStereoTypes("Target.*")
                .DependencyFrom("[Component1]").To("[Component3]")
                .DependencyFrom("[Component2]").To("[Component3]")
                .Write();

            PlantUmlDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component3 }, component1.Dependencies);
            Assert.Equal(new[] { component3 }, component2.Dependencies);
        }

        [Fact]
        public void ParseAComponentDiagramWithBothAliasAndNamesUsed()
        {
            PlantUmlDiagram diagram = CreateDiagram(TestDiagram.From(_memoryStream)
                .Component("A").WithAlias("foo").WithStereoTypes(".Service.*")
                .Component("B").WithStereoTypes("Controller.*")
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
            TestDiagram.From(_memoryStream)
                .Component("A").WithAlias("foo").WithStereoTypes("Origin.*")
                .Component("B").WithAlias("bar").WithStereoTypes("Target.*")
                .DependencyFrom("foo").To("bar")
                .DependencyFrom("[foo]").To("[bar]")
                .Write();

            PlantUmlDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent foo = GetComponentWithAlias(new Alias("foo"), diagram);
            PlantUmlComponent bar = GetComponentWithAlias(new Alias("bar"), diagram);

            Assert.Equal(new[] { bar }, foo.Dependencies);
            Assert.Empty(bar.Dependencies);
        }

        [Fact]
        public void ParsesASimpleComponentWithFile()
        {
            string path = Path.Combine(Path.GetTempPath(), "plantuml_diagram_" + Guid.NewGuid() + ".puml");
            using (FileStream fileStream = File.Create(path))
            {
                TestDiagram.From(fileStream)
                    .Component("SomeOrigin").WithStereoTypes("Origin.*")
                    .Write();
            }

            PlantUmlDiagram diagram = CreateDiagram(path);

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(origin.Stereotypes.SingleOrDefault(), new Stereotype("Origin.*"));
            Assert.Null(origin.Alias);
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
            return _parser.Parse(path);
        }

        private PlantUmlDiagram CreateDiagram(Stream stream)
        {
            return _parser.Parse(stream);
        }
    }

    public class DependencyArrowTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<string> arrowCenters = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                arrowCenters.Add(new string('-', i));
            }
            for (int i = 2; i <= 10; i++)
            {
                foreach (string infix in new string[] { "left", "right", "up", "down", "[#green]" })
                {
                    arrowCenters.Add(new string('-', i - 1) + infix + "-");
                }
            }
            foreach (string arrowCenter in arrowCenters)
            {
                yield return new object[] { "[SomeOrigin] " + arrowCenter + "> [SomeTarget]" };
                yield return new object[] { "[SomeTarget] <" + arrowCenter + " [SomeOrigin]" };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SimpleDiagramTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Func<TestDiagram, TestDiagram> func1 = (diagram) => diagram.DependencyFrom("[SomeOrigin]").To("[SomeTarget]");
            yield return new object[] { func1 };
            Func<TestDiagram, TestDiagram> func2 = (diagram) => diagram.DependencyTo("[SomeTarget]").From("[SomeOrigin]");
            yield return new object[] { func2 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
