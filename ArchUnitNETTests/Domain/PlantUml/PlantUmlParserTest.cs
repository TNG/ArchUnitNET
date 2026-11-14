using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using ArchUnitNET.Domain.PlantUml.Import;
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
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Component("SomeTarget")
                    .WithStereoTypes("Target.*")
                    .Write()
            );

            Assert.Equal(2, diagram.AllComponents.Count);
        }

        [Fact]
        public void ParsesASimpleComponent()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Write()
            );

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(origin.Stereotypes.SingleOrDefault(), new Stereotype("Origin.*"));
            Assert.Null(origin.Alias);
        }

        [Theory]
        [ClassData(typeof(SimpleDiagramTestData))]
        public void ParsesDependencyOfSimpleComponentDiagram(
            Func<TestDiagram, TestDiagram> testCaseFunc
        )
        {
            TestDiagram initialDiagram = TestDiagram
                .From(_memoryStream)
                .Component("SomeOrigin")
                .WithStereoTypes("Origin.*")
                .Component("SomeTarget")
                .WithStereoTypes("Target.*");
            PlantUmlParsedDiagram diagram = CreateDiagram(testCaseFunc(initialDiagram).Write());

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
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Component("SomeTarget")
                    .WithStereoTypes("Target.*")
                    .RawLine(dependency)
                    .Write()
            );

            PlantUmlComponent component = GetComponentWithName("SomeOrigin", diagram);
            PlantUmlComponent target = component.Dependencies.Single();

            Assert.Equal(target.ComponentName, new ComponentName("SomeTarget"));
        }

        [Theory]
        [ClassData(typeof(ComponentFormattingTestData))]
        public void ParsesComponentsWithColors(string componentLine, string expectedAlias)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram.From(_memoryStream).RawLine(componentLine).Write()
            );

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(new ComponentName("Some Component"), component.ComponentName);
            Assert.Contains(new Stereotype("Some.*"), component.Stereotypes);
            Assert.Equal(expectedAlias != null ? new Alias(expectedAlias) : null, component.Alias);
        }

        [Fact]
        public void ParsesDependenciesBetweenColoredComponents()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithColor("#palegreen")
                    .WithStereoTypes("A.*")
                    .Component("B")
                    .WithColor("#palegreen")
                    .WithStereoTypes("B.*")
                    .RawLine("B-u->A")
                    .Write()
            );

            PlantUmlComponent b = GetComponentWithName("B", diagram);
            Assert.Equal(new ComponentName("A"), b.Dependencies.Single().ComponentName);
        }

        [Theory]
        [InlineData("[Some Origin] --> [Some Target]")]
        [InlineData("[Some Target] <-l- [Some Origin]")]
        [InlineData("[Some Origin]-->[Some Target]")]
        public void ParsesDependenciesBetweenComponentNamesWithSpaces(string dependency)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("Some Origin")
                    .WithStereoTypes("Origin.*")
                    .Component("Some Target")
                    .WithStereoTypes("Target.*")
                    .RawLine(dependency)
                    .Write()
            );

            PlantUmlComponent origin = GetComponentWithName("Some Origin", diagram);
            Assert.Equal(
                new ComponentName("Some Target"),
                origin.Dependencies.Single().ComponentName
            );
        }

        [Theory]
        [InlineData("some-origin --> some-target")]
        [InlineData("some-origin-->some-target")]
        [InlineData("some-target<-u-some-origin")]
        public void ParsesDependenciesBetweenAliasesWithHyphens(string dependency)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithAlias("some-origin")
                    .WithStereoTypes("Origin.*")
                    .Component("SomeTarget")
                    .WithAlias("some-target")
                    .WithStereoTypes("Target.*")
                    .RawLine(dependency)
                    .Write()
            );

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(
                new ComponentName("SomeTarget"),
                origin.Dependencies.Single().ComponentName
            );
        }

        [Theory]
        [InlineData("[A] <--> [B]")]
        [InlineData("[A] <-[#red]> [B]")]
        public void ParsesArrowsWithHeadsOnBothEndsAsDependenciesInBothDirections(string dependency)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithStereoTypes("A.*")
                    .Component("B")
                    .WithStereoTypes("B.*")
                    .RawLine(dependency)
                    .Write()
            );

            PlantUmlComponent a = GetComponentWithName("A", diagram);
            PlantUmlComponent b = GetComponentWithName("B", diagram);
            Assert.Equal(new[] { b }, a.Dependencies);
            Assert.Equal(new[] { a }, b.Dependencies);
        }

        [Theory]
        [ClassData(typeof(LinkTypeTestData))]
        public void ParsesDependenciesFromLinkTypes(string link, string[] expectedDependencies)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithStereoTypes("A.*")
                    .Component("B")
                    .WithStereoTypes("B.*")
                    .RawLine(link)
                    .Write()
            );

            Assert.Equal(expectedDependencies, GetDependencies(diagram));
        }

        [Theory]
        [InlineData("[A] -[hidden]-> [B]")]
        [InlineData("[A] -[hidden]> [B]")]
        [InlineData("[B] <-[hidden]- [A]")]
        [InlineData("A -[hidden]--> B")]
        [InlineData("A -[#red,hidden]-> B")]
        [InlineData("A -[#red, hidden]-> B")]
        [InlineData("A -[hidden]up-> B")]
        [InlineData("A -up[hidden]-> B")]
        [InlineData("A *-[hidden]- B")]
        [InlineData("A <-[hidden]-> B")]
        public void IgnoresHiddenLinks(string link)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithStereoTypes("A.*")
                    .Component("B")
                    .WithStereoTypes("B.*")
                    .RawLine(link)
                    .Write()
            );

            Assert.Empty(GetDependencies(diagram));
        }

        [Theory]
        [InlineData("box-->oak", new[] { "Box->Oak" })]
        [InlineData("oak<--box", new[] { "Box->Oak" })]
        [InlineData("box--oak", new string[0])]
        [InlineData("oak--box", new string[0])]
        [InlineData("box --o oak", new[] { "Oak->Box" })]
        [InlineData("box o-- oak", new[] { "Box->Oak" })]
        [InlineData("box--x0", new string[0])]
        [InlineData("x0--box", new string[0])]
        [InlineData("x0 --x box", new string[0])]
        [InlineData("x0 0)-- box", new string[0])]
        public void ParsesDecorationLettersOnlyIfSeparatedFromNames(
            string link,
            string[] expectedDependencies
        )
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("Box")
                    .WithAlias("box")
                    .WithStereoTypes("Box.*")
                    .Component("Oak")
                    .WithAlias("oak")
                    .WithStereoTypes("Oak.*")
                    .Component("X0")
                    .WithAlias("x0")
                    .WithStereoTypes("X0.*")
                    .RawLine(link)
                    .Write()
            );

            Assert.Equal(expectedDependencies, GetDependencies(diagram));
        }

        [Fact]
        public void DoesNotIncludeCommentedOutLines()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("uncommentedComponent")
                    .WithAlias("uncommentedAlias")
                    .WithStereoTypes("UncommentedNamespace.*")
                    .RawLine("  '  [commentedComponent] <<CommentedNamespace.*>> as commentedAlias")
                    .RawLine("")
                    .RawLine(" ' [uncommentedComponent] --> [commentedComponent]")
                    .Write()
            );

            PlantUmlComponent uncommentedComponent = GetComponentWithName(
                "uncommentedComponent",
                diagram
            );

            Assert.Equal(diagram.AllComponents.Single(), uncommentedComponent);
            Assert.Empty(uncommentedComponent.Dependencies);
        }

        [Fact]
        public void DoesNotIncludeDependencyDescriptions()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("component")
                    .WithStereoTypes("SomeNamespace.*")
                    .Component("otherComponent")
                    .WithStereoTypes("SomeNamespace2.*")
                    .RawLine(
                        "[component] --> [otherComponent] : this part should be ignored, no matter the comment tick ' "
                    )
                    .Write()
            );

            PlantUmlComponent component = GetComponentWithName("component", diagram);
            PlantUmlComponent targetOfDescribedDependency = component.Dependencies.Single();
            Assert.Equal(
                targetOfDescribedDependency.ComponentName,
                new ComponentName("otherComponent")
            );
        }

        [Fact]
        public void ThrowsExceptionWithComponentsThatAreNotYetDefined()
        {
            TestDiagram
                .From(_memoryStream)
                .DependencyFrom("[NotYetDefined]")
                .To("[AlsoNotYetDefined]")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() =>
                CreateDiagram(_memoryStream)
            );
            Assert.Contains(
                "There is no Component with name or alias = 'NotYetDefined'",
                exception.Message
            );
            Assert.Contains(
                "Components must be specified separately from dependencies",
                exception.Message
            );
        }

        [Fact]
        public void ThrowsExceptionWithComponentsWithoutStereotypes()
        {
            TestDiagram.From(_memoryStream).RawLine("[componentWithoutStereotype]").Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() =>
                CreateDiagram(_memoryStream)
            );
            Assert.Contains("componentWithoutStereotype", exception.Message);
            Assert.Contains(
                "at least one stereotype specifying the namespace identifier (<<.*>>)",
                exception.Message
            );
        }

        [Fact]
        public void ParsesTwoIdenticalComponentsNoDependency()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("someName")
                    .WithAlias("someAlias")
                    .WithStereoTypes("someStereotype")
                    .Component("someName")
                    .WithAlias("someAlias")
                    .WithStereoTypes("someStereotype")
                    .Write()
            );

            Assert.Equal(
                new[] { GetComponentWithName("someName", diagram) },
                diagram.AllComponents
            );
        }

        [Fact]
        public void RejectsAComponentWithAnIllegalAlias()
        {
            TestDiagram
                .From(_memoryStream)
                .Component("irrelevant")
                .WithAlias("ill[]egal")
                .WithStereoTypes("Irrelevant.*")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() =>
                CreateDiagram(_memoryStream)
            );
            Assert.Contains(
                "Alias 'ill[]egal' should not contain character(s): '[' or ']' or '\"'",
                exception.Message
            );
        }

        [Fact]
        public void ParsesATrickyAlias()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("tricky")
                    .WithAlias("because it's quoted")
                    .WithStereoTypes("Tricky.*")
                    .Component("tricky as hell cause of as keyword")
                    .WithAlias("other")
                    .WithStereoTypes("Other.*")
                    .Write()
            );

            PlantUmlComponent trickyAsHell = GetComponentWithName(
                "tricky as hell cause of as keyword",
                diagram
            );
            PlantUmlComponent tricky = GetComponentWithName("tricky", diagram);

            Assert.Equal(new Alias("other"), trickyAsHell.Alias);
            Assert.Equal(new Alias("because it's quoted"), tricky.Alias);
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypes()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("someComponent")
                    .WithStereoTypes("FirstNamespace.*", "SecondNamespace.*", "ThirdNamespace.*")
                    .Write()
            );

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(
                new[]
                {
                    new Stereotype("FirstNamespace.*"),
                    new Stereotype("SecondNamespace.*"),
                    new Stereotype("ThirdNamespace.*"),
                },
                component.Stereotypes.OrderBy(st => st.AsString())
            );
        }

        [Fact]
        public void ParsesComponentDiagramWithMultipleStereotypesAndAlias()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("someComponent")
                    .WithAlias("someAlias")
                    .WithStereoTypes("FirstNamespace.*", "SecondNamespace.*", "ThirdNamespace.*")
                    .Write()
            );

            PlantUmlComponent component = diagram.AllComponents.Single();

            Assert.Equal(new Alias("someAlias"), component.Alias);
        }

        [Fact]
        public void ParsesDiagramWithDependenciesThatUseAlias()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithAlias("aliasForA")
                    .WithStereoTypes("Controller.*")
                    .Component("B")
                    .WithAlias("aliasForB")
                    .WithStereoTypes("Service.*")
                    .DependencyFrom("aliasForA")
                    .To("aliasForB")
                    .Write()
            );

            PlantUmlComponent aliasForA = GetComponentWithAlias(new Alias("aliasForA"), diagram);
            PlantUmlComponent aliasForB = GetComponentWithAlias(new Alias("aliasForB"), diagram);

            Assert.Equal(aliasForA, GetComponentWithName("A", diagram));
            Assert.Equal(aliasForB, GetComponentWithName("B", diagram));
            Assert.Equal(new[] { aliasForB }, aliasForA.Dependencies);
        }

        [Fact]
        public void ParsesDependenciesBetweenComponentsWithoutBrackets()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithStereoTypes("Origin.*")
                    .Component("B")
                    .WithStereoTypes("Target.*")
                    .DependencyFrom("A")
                    .To("B")
                    .Write()
            );

            PlantUmlComponent a = GetComponentWithName("A", diagram);
            PlantUmlComponent b = GetComponentWithName("B", diagram);

            Assert.Equal(new[] { b }, a.Dependencies);
        }

        [Fact]
        public void ParsesMultipleComponentsAndDependencies()
        {
            TestDiagram
                .From(_memoryStream)
                .Component("Component1")
                .WithStereoTypes("Origin1.*")
                .Component("Component2")
                .WithStereoTypes("Target1.*")
                .Component("Component3")
                .WithStereoTypes("Origin2.*")
                .Component("Component4")
                .WithStereoTypes("Target2.*")
                .DependencyFrom("Component1")
                .To("Component2")
                .DependencyFrom("Component3")
                .To("Component4")
                .Write();

            PlantUmlParsedDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);
            PlantUmlComponent component4 = GetComponentWithName("Component4", diagram);

            Assert.Equal(
                new[] { component1, component2, component3, component4 },
                diagram.AllComponents.OrderBy(c => c.ComponentName.AsString())
            );
            Assert.Equal(new[] { component2 }, component1.Dependencies);
            Assert.Empty(component2.Dependencies);
            Assert.Equal(new[] { component4 }, component3.Dependencies);
            Assert.Empty(component4.Dependencies);
        }

        [Fact]
        public void ParsesADiagramWithNoUniqueOrigins()
        {
            TestDiagram
                .From(_memoryStream)
                .Component("Component1")
                .WithStereoTypes("Origin.*")
                .Component("Component2")
                .WithStereoTypes("Target1.*")
                .Component("Component3")
                .WithStereoTypes("Target2.*")
                .DependencyFrom("[Component1]")
                .To("[Component2]")
                .DependencyFrom("[Component1]")
                .To("[Component3]")
                .Write();

            PlantUmlParsedDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component2, component3 }, component1.Dependencies);
        }

        [Fact]
        public void ParseADiagramWithNonUniqueTargets()
        {
            TestDiagram
                .From(_memoryStream)
                .Component("Component1")
                .WithStereoTypes("Origin1.*")
                .Component("Component2")
                .WithStereoTypes("Origin2.*")
                .Component("Component3")
                .WithStereoTypes("Target.*")
                .DependencyFrom("[Component1]")
                .To("[Component3]")
                .DependencyFrom("[Component2]")
                .To("[Component3]")
                .Write();

            PlantUmlParsedDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent component1 = GetComponentWithName("Component1", diagram);
            PlantUmlComponent component2 = GetComponentWithName("Component2", diagram);
            PlantUmlComponent component3 = GetComponentWithName("Component3", diagram);

            Assert.Equal(new[] { component3 }, component1.Dependencies);
            Assert.Equal(new[] { component3 }, component2.Dependencies);
        }

        [Fact]
        public void ParseAComponentDiagramWithBothAliasAndNamesUsed()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("A")
                    .WithAlias("foo")
                    .WithStereoTypes(".Service.*")
                    .Component("B")
                    .WithStereoTypes("Controller.*")
                    .DependencyFrom("[B]")
                    .To("foo")
                    .DependencyFrom("foo")
                    .To("[B]")
                    .Write()
            );

            PlantUmlComponent componentB = GetComponentWithName("B", diagram);
            PlantUmlComponent componentFoo = GetComponentWithAlias(new Alias("foo"), diagram);

            Assert.Equal(new[] { componentFoo }, componentB.Dependencies);
            Assert.Equal(new[] { componentB }, componentFoo.Dependencies);
        }

        [Fact]
        public void ParsesAComponentDiagramThatUsesAliasWithAndWithoutBrackets()
        {
            TestDiagram
                .From(_memoryStream)
                .Component("A")
                .WithAlias("foo")
                .WithStereoTypes("Origin.*")
                .Component("B")
                .WithAlias("bar")
                .WithStereoTypes("Target.*")
                .DependencyFrom("foo")
                .To("bar")
                .DependencyFrom("[foo]")
                .To("[bar]")
                .Write();

            PlantUmlParsedDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent foo = GetComponentWithAlias(new Alias("foo"), diagram);
            PlantUmlComponent bar = GetComponentWithAlias(new Alias("bar"), diagram);

            Assert.Equal(new[] { bar }, foo.Dependencies);
            Assert.Empty(bar.Dependencies);
        }

        [Fact]
        public void ParsesAComponentDiagramThatUsesQuotedAliasWithAndWithoutBrackets()
        {
            TestDiagram
                .From(_memoryStream)
                .RawLine("[A] <<Origin.*>> as \"foo\"")
                .RawLine("[B] <<Target.*>> as \"bar\"")
                .DependencyFrom("foo")
                .To("bar")
                .DependencyFrom("[foo]")
                .To("[bar]")
                .Write();

            PlantUmlParsedDiagram diagram = CreateDiagram(_memoryStream);

            PlantUmlComponent foo = GetComponentWithAlias(new Alias("foo"), diagram);
            PlantUmlComponent bar = GetComponentWithAlias(new Alias("bar"), diagram);

            Assert.Equal(new[] { bar }, foo.Dependencies);
            Assert.Empty(bar.Dependencies);
        }

        [Theory]
        [InlineData("[A] <<X>>", "A", null, new[] { "X" })]
        [InlineData("[A]<<X>>", "A", null, new[] { "X" })]
        [InlineData("[A] <<X>><<Y>>", "A", null, new[] { "X", "Y" })]
        [InlineData("[A] <<X>>   <<Y>> <<Z>>", "A", null, new[] { "X", "Y", "Z" })]
        [InlineData("[A B] <<X.*>>", "A B", null, new[] { "X.*" })]
        [InlineData("[A] <<X>> as B", "A", "B", new[] { "X" })]
        [InlineData("[A] <<X>> as \"B\"", "A", "B", new[] { "X" })]
        [InlineData("[A] <<X>> as \"B C\"", "A", "B C", new[] { "X" })]
        [InlineData("[A] <<X>> <<Y>> as \"B\"", "A", "B", new[] { "X", "Y" })]
        [InlineData("   [A] <<X>>", "A", null, new[] { "X" })]
        [InlineData("[A] <<X>>   ", "A", null, new[] { "X" })]
        [InlineData("\t[A] <<X>> as \"B\"\t", "A", "B", new[] { "X" })]
        public void ParsesComponentLineVariants(
            string line,
            string expectedName,
            string expectedAlias,
            string[] expectedStereotypes
        )
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram.From(_memoryStream).RawLine(line).Write()
            );

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(new ComponentName(expectedName), component.ComponentName);
            Assert.Equal(expectedAlias != null ? new Alias(expectedAlias) : null, component.Alias);
            Assert.Equal(
                expectedStereotypes.Select(s => new Stereotype(s)),
                component.Stereotypes.OrderBy(s => s.AsString())
            );
        }

        [Fact]
        public void DoesNotReadStereotypesFromTheAlias()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram.From(_memoryStream).RawLine("[A] <<Real.*>> as \"<<Fake.*>>\"").Write()
            );

            PlantUmlComponent component = diagram.AllComponents.Single();
            Assert.Equal(new[] { new Stereotype("Real.*") }, component.Stereotypes);
            Assert.Equal(new Alias("<<Fake.*>>"), component.Alias);
        }

        [Theory]
        [InlineData("[A <<X>>")]
        [InlineData("A] <<X>>")]
        [InlineData("[[A]] <<X>>")]
        [InlineData("[A] <<X>")]
        [InlineData("[A] <<X>> B")]
        [InlineData("[A] <<X>> as \"B\" C")]
        [InlineData("B [A] <<X>>")]
        public void IgnoresLinesThatAreNotComponents(string line)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram.From(_memoryStream).RawLine(line).Write()
            );

            Assert.Empty(diagram.AllComponents);
        }

        [Theory]
        [ClassData(typeof(DependencyLineVariantsTestData))]
        public void ParsesDependencyLineVariants(string dependency)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Component("SomeTarget")
                    .WithStereoTypes("Target.*")
                    .RawLine(dependency)
                    .Write()
            );

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            PlantUmlComponent target = GetComponentWithName("SomeTarget", diagram);

            Assert.Equal(new[] { target }, origin.Dependencies);
            Assert.Empty(target.Dependencies);
        }

        [Theory]
        [InlineData("[SomeOrigin] -- [SomeTarget]")]
        [InlineData("[SomeOrigin] - [SomeTarget]")]
        [InlineData("[SomeOrigin] [SomeTarget]")]
        public void IgnoresLinesThatAreNotDependencies(string line)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Component("SomeTarget")
                    .WithStereoTypes("Target.*")
                    .RawLine(line)
                    .Write()
            );

            Assert.All(diagram.AllComponents, c => Assert.Empty(c.Dependencies));
        }

        [Theory]
        [InlineData("package")]
        [InlineData("node")]
        [InlineData("folder")]
        [InlineData("frame")]
        [InlineData("cloud")]
        [InlineData("database")]
        public void ComponentsInGroupKeepOnlyTheirOwnStereotypes(string keyword)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine(keyword + " \"Group\" <<GroupNs>> {")
                    .RawLine("[Inner] <<InnerNs>>")
                    .RawLine("}")
                    .Write()
            );

            Assert.Equal(
                new Stereotype("InnerNs"),
                GetComponentWithName("Inner", diagram).Stereotypes.Single()
            );
        }

        [Fact]
        public void ThrowsExceptionForComponentInGroupWithoutOwnStereotype()
        {
            TestDiagram
                .From(_memoryStream)
                .RawLine("package Group <<GroupNs>> {")
                .RawLine("[Inner]")
                .RawLine("}")
                .Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() =>
                CreateDiagram(_memoryStream)
            );
            Assert.Contains("'Inner'", exception.Message);
        }

        [Theory]
        [InlineData("package Group <<GroupNs>> #palegreen {")]
        [InlineData("package Group <<GroupNs>> #red;line:blue{")]
        [InlineData("package \"Colored Group\" as Group <<GroupNs>> #palegreen {")]
        public void ParsesColoredGroups(string groupLine)
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine(groupLine)
                    .RawLine("[Inner] <<InnerNs>>")
                    .RawLine("}")
                    .RawLine("Group --> [Target]")
                    .Component("Target")
                    .WithStereoTypes("TargetNs")
                    .Write()
            );

            Assert.Equal(new[] { "Inner->Target" }, GetDependencies(diagram));
        }

        [Fact]
        public void DependencyBetweenGroupsAppliesToAllNestedComponents()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine("package \"Outer\" as outer {")
                    .RawLine("node inner {")
                    .RawLine("[A] <<A>>")
                    .RawLine("}")
                    .RawLine("[B] <<B>>")
                    .RawLine("}")
                    .RawLine("package target {")
                    .RawLine("[C] <<C>>")
                    .RawLine("}")
                    .RawLine("outer --> target")
                    .Write()
            );

            Assert.Equal(
                new[] { "C" },
                GetComponentWithName("A", diagram)
                    .Dependencies.Select(d => d.ComponentName.AsString())
            );
            Assert.Equal(
                new[] { "C" },
                GetComponentWithName("B", diagram)
                    .Dependencies.Select(d => d.ComponentName.AsString())
            );
            Assert.Empty(GetComponentWithName("C", diagram).Dependencies);
        }

        [Theory]
        [InlineData("[Shared] --> [Target]", "Shared")]
        [InlineData("Shared --> [Target]", "Inner")]
        public void BracketedEndpointRefersToComponentEvenIfAGroupHasTheSameName(
            string dependency,
            string expectedOrigin
        )
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine("package Shared {")
                    .RawLine("[Inner] <<Inner>>")
                    .RawLine("}")
                    .Component("Shared")
                    .WithStereoTypes("Shared")
                    .Component("Target")
                    .WithStereoTypes("Target")
                    .RawLine(dependency)
                    .Write()
            );

            Assert.Equal(
                new[] { expectedOrigin },
                diagram
                    .AllComponents.Where(component => component.Dependencies.Any())
                    .Select(component => component.ComponentName.AsString())
            );
        }

        [Fact]
        public void ThrowsExceptionForUnclosedGroup()
        {
            TestDiagram.From(_memoryStream).RawLine("package Group <<Ns>> {").Write();

            IllegalDiagramException exception = Assert.Throws<IllegalDiagramException>(() =>
                CreateDiagram(_memoryStream)
            );
            Assert.Contains("never closed", exception.Message);
        }

        [Fact]
        public void IgnoresBracesOfNonGroupBlocks()
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine("skinparam component {")
                    .RawLine("BackgroundColor white")
                    .RawLine("}")
                    .RawLine("[Outside] <<Outside>>")
                    .Write()
            );

            Assert.Equal(
                new Stereotype("Outside"),
                GetComponentWithName("Outside", diagram).Stereotypes.Single()
            );
        }

        [Theory]
        [InlineData("[Origin] --> [Shared]", "Shared")]
        [InlineData("[Origin] --> Shared", "Inner")]
        [InlineData("[Shared] <-- [Origin]", "Shared")]
        [InlineData("Shared <-- [Origin]", "Inner")]
        public void BracketedTargetRefersToComponentEvenIfAGroupHasTheSameName(
            string dependency,
            string expectedTarget
        )
        {
            PlantUmlParsedDiagram diagram = CreateDiagram(
                TestDiagram
                    .From(_memoryStream)
                    .RawLine("package Shared {")
                    .RawLine("[Inner] <<Inner>>")
                    .RawLine("}")
                    .Component("Shared")
                    .WithStereoTypes("Shared")
                    .Component("Origin")
                    .WithStereoTypes("Origin")
                    .RawLine(dependency)
                    .Write()
            );

            Assert.Equal(new[] { "Origin->" + expectedTarget }, GetDependencies(diagram));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullFilename()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse((string)null));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionForNullStream()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse((Stream)null));
        }

        [Fact]
        public void ThrowsParseExceptionForMissingFile()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                "plantuml_diagram_" + Guid.NewGuid() + ".puml"
            );

            PlantUmlParseException exception = Assert.Throws<PlantUmlParseException>(() =>
                _parser.Parse(path)
            );
            Assert.IsAssignableFrom<IOException>(exception.InnerException);
        }

        [Fact]
        public void ThrowsParseExceptionForUnreadableStream()
        {
            PlantUmlParseException exception = Assert.Throws<PlantUmlParseException>(() =>
                _parser.Parse(new UnreadableStream())
            );
            Assert.IsAssignableFrom<IOException>(exception.InnerException);
        }

        [Fact]
        public void ParsesASimpleComponentWithFile()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                "plantuml_diagram_" + Guid.NewGuid() + ".puml"
            );
            using (FileStream fileStream = File.Create(path))
            {
                TestDiagram
                    .From(fileStream)
                    .Component("SomeOrigin")
                    .WithStereoTypes("Origin.*")
                    .Write();
            }

            PlantUmlParsedDiagram diagram = CreateDiagram(path);

            PlantUmlComponent origin = GetComponentWithName("SomeOrigin", diagram);
            Assert.Equal(origin.Stereotypes.SingleOrDefault(), new Stereotype("Origin.*"));
            Assert.Null(origin.Alias);
        }

        private PlantUmlComponent GetComponentWithName(
            string componentName,
            PlantUmlParsedDiagram diagram
        )
        {
            PlantUmlComponent component = diagram
                .AllComponents.Where(comp =>
                    Equals(comp.ComponentName, new ComponentName(componentName))
                )
                .First();
            return component;
        }

        private static IEnumerable<string> GetDependencies(PlantUmlParsedDiagram diagram)
        {
            return diagram
                .AllComponents.SelectMany(origin =>
                    origin.Dependencies.Select(target =>
                        origin.ComponentName.AsString() + "->" + target.ComponentName.AsString()
                    )
                )
                .OrderBy(dependency => dependency);
        }

        private PlantUmlComponent GetComponentWithAlias(Alias alias, PlantUmlParsedDiagram diagram)
        {
            return diagram.ComponentsWithAlias.Where(c => alias.Equals(c.Alias)).Single();
        }

        private PlantUmlParsedDiagram CreateDiagram(string path)
        {
            return _parser.Parse(path);
        }

        private PlantUmlParsedDiagram CreateDiagram(Stream stream)
        {
            return _parser.Parse(stream);
        }

        private class UnreadableStream : Stream
        {
            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new IOException("Simulated read failure");
            }

            public override void Flush() { }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }

    public class DependencyLineVariantsTestData : IEnumerable<object[]>
    {
        // Complements DependencyArrowTestData, which covers every arrow center between
        // bracketed endpoints without a description
        public IEnumerator<object[]> GetEnumerator()
        {
            string[] arrowCenters = { "-", "--", "-left-", "-[#green]-" };
            foreach (bool bracketed in new[] { true, false })
            {
                foreach (bool described in new[] { true, false })
                {
                    if (bracketed && !described)
                    {
                        continue;
                    }
                    string origin = bracketed ? "[SomeOrigin]" : "SomeOrigin";
                    string target = bracketed ? "[SomeTarget]" : "SomeTarget";
                    string description = described ? " : some description" : "";
                    foreach (string arrowCenter in arrowCenters)
                    {
                        yield return new object[]
                        {
                            origin + " " + arrowCenter + "> " + target + description,
                        };
                        yield return new object[]
                        {
                            target + " <" + arrowCenter + " " + origin + description,
                        };
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class DependencyArrowTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var rightArrows = new List<string>();
            var leftArrows = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                rightArrows.Add(new string('-', i) + ">");
                leftArrows.Add("<" + new string('-', i));
            }
            for (int i = 2; i <= 10; i++)
            {
                foreach (string infix in new[] { "left", "right", "up", "down", "[#green]" })
                {
                    rightArrows.Add(new string('-', i - 1) + infix + "->");
                    leftArrows.Add("<-" + infix + new string('-', i - 1));
                }
            }
            foreach (
                string infix in new[]
                {
                    "l",
                    "r",
                    "u",
                    "d",
                    "le",
                    "ri",
                    "do",
                    "[#red,dashed]",
                    "[#red]up",
                    "up[#red]",
                    "[#red]u",
                }
            )
            {
                rightArrows.Add("-" + infix + "->");
                leftArrows.Add("<-" + infix + "-");
            }
            // style without trailing dash, as written by PlantUmlFileBuilder
            rightArrows.Add("-[#red]>");
            leftArrows.Add("<[#red]-");

            foreach (string separator in new[] { " ", "" })
            {
                foreach (string arrow in rightArrows)
                {
                    yield return new object[]
                    {
                        "[SomeOrigin]" + separator + arrow + separator + "[SomeTarget]",
                    };
                }
                foreach (string arrow in leftArrows)
                {
                    yield return new object[]
                    {
                        "[SomeTarget]" + separator + arrow + separator + "[SomeOrigin]",
                    };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class LinkTypeTestData : IEnumerable<object[]>
    {
        // the link types shown on https://plantuml.com/class-diagram,
        // https://plantuml.com/component-diagram and https://plantuml.com/deployment-diagram
        private static readonly string[] FromAToB =
        {
            "-->",
            "->",
            "..>",
            "==>",
            "~~>",
            "-->>",
            "--|>",
            "..|>",
            "--||>",
            "--:|>",
            "--^",
            "--\\\\",
            "*--",
            "o--",
            "*..",
            "o..",
            "*-->",
            "#-->>",
            "*-0)-+",
            "-le(0)->",
            ".up(0).>",
            "~up(0)~>",
            "-ri0)->",
            "-[#red]>",
            "-[#red,dashed,thickness=2]->",
        };

        private static readonly string[] FromBToA =
        {
            "<--",
            "<-",
            "<..",
            "<|--",
            "<|..",
            "<<--",
            "^--",
            "--*",
            "--o",
            "..o",
            "<--*",
            "<--o",
            "<-[#blue]-",
        };

        private static readonly string[] InBothDirections =
        {
            "<-->",
            "<..>",
            "<-0->",
            "<-(0)->",
            "<-->>",
            "<|--|>",
            "^--^",
            "*--*",
            "o--o",
            "*--o",
        };

        private static readonly string[] WithoutDirection =
        {
            "--",
            "-",
            "..",
            "==",
            "~~",
            "-up-",
            "-[#red]-",
            "#--",
            "x--",
            "}--",
            "+--",
            "@--",
            "0--",
            ")--",
            "0)--",
            "()--",
            "--#",
            "--x",
            "--{",
            "--+",
            "--@",
            "--0",
            "--(",
            "--(0",
            "--()",
            "-0-",
            "-0)-",
            "-(0-",
            "-(0)-",
            "0)-(0-(0",
            ")-0-(0",
            "#~~(",
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            var expectations = new[]
            {
                (FromAToB, new[] { "A->B" }),
                (FromBToA, new[] { "B->A" }),
                (InBothDirections, new[] { "A->B", "B->A" }),
                (WithoutDirection, new string[0]),
            };
            foreach ((string[] links, string[] expectedDependencies) in expectations)
            {
                foreach (string link in links)
                {
                    yield return new object[] { "[A] " + link + " [B]", expectedDependencies };
                    yield return new object[] { "A " + link + " B", expectedDependencies };
                    yield return new object[] { "[A]" + link + "[B]", expectedDependencies };
                }
            }
            yield return new object[] { "[A] \"1\" *-- \"many\" [B] : contains", new[] { "A->B" } };
            yield return new object[] { "A --> \"1\" B", new[] { "A->B" } };
            yield return new object[] { "[A] \"1\" --|> \"many\" [B]", new[] { "A->B" } };
            yield return new object[]
            {
                "A --> B #line:red;line.bold;text:red  : red bold",
                new[] { "A->B" },
            };
            yield return new object[] { "A - B : drives >", new string[0] };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ComponentFormattingTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (string color in new[] { "#palegreen", "#99ffd1", "#red;line:blue" })
            {
                // line, expected alias
                yield return new object[] { "[Some Component] <<Some.*>> " + color, null };
                yield return new object[] { "[Some Component] <<Some.*>> " + color + "  ", null };
                yield return new object[]
                {
                    "[Some Component] <<Some.*>> as someAlias " + color,
                    "someAlias",
                };
                yield return new object[]
                {
                    "[Some Component] <<Some.*>> as \"some alias\" " + color,
                    "some alias",
                };
                yield return new object[]
                {
                    "[Some Component] <<Some.*>> as some-alias " + color,
                    "some-alias",
                };
                yield return new object[]
                {
                    "[Some Component] <<Some.*>> <<Other.*>> as someAlias " + color,
                    "someAlias",
                };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SimpleDiagramTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Func<TestDiagram, TestDiagram> func1 = (diagram) =>
                diagram.DependencyFrom("[SomeOrigin]").To("[SomeTarget]");
            yield return new object[] { func1 };
            Func<TestDiagram, TestDiagram> func2 = (diagram) =>
                diagram.DependencyTo("[SomeTarget]").From("[SomeOrigin]");
            yield return new object[] { func2 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
