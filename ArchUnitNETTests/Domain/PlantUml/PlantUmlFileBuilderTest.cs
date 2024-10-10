//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using ArchUnitNET.Domain.PlantUml.Export;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
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
        public void BuildUmlByDependenciesTest()
        {
            var builder = new PlantUmlFileBuilder().WithElements(Dependencies);
            var uml = builder.AsString();
            Assert.NotEmpty(uml);

            var expectedUml =
                "@startuml"
                + Environment.NewLine
                + Environment.NewLine
                + "!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml"
                + Environment.NewLine
                + Environment.NewLine
                + "HIDE_STEREOTYPE()"
                + Environment.NewLine
                + Environment.NewLine
                + "[a] --|> [b]"
                + Environment.NewLine
                + "[b] --|> [c]"
                + Environment.NewLine
                + "[c] --|> [a]"
                + Environment.NewLine
                + "@enduml"
                + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }

        [Fact]
        public void BuildUmlByDependenciesWithObjectsWithNoDependenciesTest()
        {
            var classesWithoutDependencies = new[] { new PlantUmlClass("d") };
            var builder = new PlantUmlFileBuilder().WithElements(
                Dependencies.Concat(classesWithoutDependencies)
            );
            var uml = builder.AsString();
            Assert.NotEmpty(uml);

            var expectedUml =
                "@startuml"
                + Environment.NewLine
                + Environment.NewLine
                + "!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml"
                + Environment.NewLine
                + Environment.NewLine
                + "HIDE_STEREOTYPE()"
                + Environment.NewLine
                + Environment.NewLine
                + "class \"d\" {"
                + Environment.NewLine
                + "}"
                + Environment.NewLine
                + "[a] --|> [b]"
                + Environment.NewLine
                + "[b] --|> [c]"
                + Environment.NewLine
                + "[c] --|> [a]"
                + Environment.NewLine
                + "@enduml"
                + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }

        [Fact]
        public void HandleIllegalComponentNamesTest()
        {
            var forbiddenCharacters = new[] { "[", "]", "\r", "\n", "\f", "\a", "\b", "\v" };
            foreach (var character in forbiddenCharacters)
            {
                Assert.Throws<IllegalComponentNameException>(
                    () => new PlantUmlDependency(character, "a", DependencyType.OneToOne)
                );
                Assert.Throws<IllegalComponentNameException>(
                    () => new PlantUmlDependency("a", character, DependencyType.OneToOne)
                );
                Assert.Throws<IllegalComponentNameException>(() => new PlantUmlClass(character));
                Assert.Throws<IllegalComponentNameException>(
                    () => new PlantUmlInterface(character)
                );
                Assert.Throws<IllegalComponentNameException>(() => new PlantUmlSlice(character));
                Assert.Throws<IllegalComponentNameException>(
                    () => new PlantUmlNamespace(character)
                );
            }
        }

        [Fact]
        public void SpecialCharactersInComponentNamesTest()
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
            var builder = new PlantUmlFileBuilder().WithElements(
                dependenciesWithSpecialCharacters.Concat(classesWithSpecialCharacters)
            );
            var uml = builder.AsString();
            Assert.NotEmpty(uml);

            var expectedUml =
                "@startuml"
                + Environment.NewLine
                + Environment.NewLine
                + "!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml"
                + Environment.NewLine
                + Environment.NewLine
                + "HIDE_STEREOTYPE()"
                + Environment.NewLine
                + Environment.NewLine
                + "class \"!§´`$%&/()=?\\\täöüß^°-*+,-.,;:<>|@€\" {"
                + Environment.NewLine
                + "}"
                + Environment.NewLine
                + "[!\"§´`] --|> [$%&/()=?]"
                + Environment.NewLine
                + "[\\\t%] --|> [äöüß]"
                + Environment.NewLine
                + "[^°-*+.,;:] --|> [<>|@€]"
                + Environment.NewLine
                + "@enduml"
                + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }
    }

    internal class ClassToFocusOn { }

    internal class DependantClassOfFocusedClass { }

    internal class DependingClassOfFocusedClass { }
}
