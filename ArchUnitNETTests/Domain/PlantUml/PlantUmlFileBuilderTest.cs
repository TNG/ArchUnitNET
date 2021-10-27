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
using ArchUnitNET.Domain.PlantUml;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    public class PlantUmlFileBuilderTest
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(PlantUmlFileBuilderTest).Assembly).Build();

        private static readonly List<PlantUmlDependency> Dependencies = new List<PlantUmlDependency>
        {
            new PlantUmlDependency("a", "b"),
            new PlantUmlDependency("b", "c"),
            new PlantUmlDependency("c", "a")
        };

        [Fact]
        public void BuildUmlByTypesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Types.Take(100));
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByTypesIncludingDependenciesToOtherTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Types.Take(100), true);
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByNamespacesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Namespaces);
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlBySlicesTest()
        {
            var slices = SliceRuleDefinition.Slices().Matching("ArchUnitNETTests.(*).").GetObjects(Architecture);
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(slices);
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByDependenciesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Dependencies);
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);

            var expectedUml = "@startuml" + Environment.NewLine + "[a] --> [b]" +
                              Environment.NewLine + "[b] --> [c]" + Environment.NewLine + "[c] --> [a]" +
                              Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }

        [Fact]
        public void BuildUmlByDependenciesWithObjectsWithNoDependenciesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Dependencies, "d", "");
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);


            var expectedUml = "@startuml" + Environment.NewLine + "[d]" + Environment.NewLine + "[a] --> [b]" +
                              Environment.NewLine + "[b] --> [c]" + Environment.NewLine + "[c] --> [a]" +
                              Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }

        [Fact]
        public void HandleIllegalComponentNamesTest()
        {
            var forbiddenCharacters = new[] { "[", "]", "\r", "\n", "\f", "\a", "\b", "\v" };
            foreach (var character in forbiddenCharacters)
            {
                var deps1 = new List<PlantUmlDependency>
                {
                    new PlantUmlDependency(character, "a"),
                };
                var deps2 = new List<PlantUmlDependency>
                {
                    new PlantUmlDependency("a", character),
                };
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlFileBuilder().WithDependenciesFrom(deps1).Build());
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlFileBuilder().WithDependenciesFrom(deps2).Build());
                Assert.Throws<IllegalComponentNameException>(() =>
                    new PlantUmlFileBuilder().WithDependenciesFrom(Dependencies, character).Build());
            }
        }


        [Fact]
        public void SpecialCharactersInComponentNamesTest()
        {
            var dependenciesWithSpecialCharacters = new List<PlantUmlDependency>
            {
                new PlantUmlDependency("!\"§´`", "$%&/()=?"),
                new PlantUmlDependency("\\\t%", "äöüß"),
                new PlantUmlDependency("^°-*+.,;:", "<>|@€")
            };
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(dependenciesWithSpecialCharacters,
                "!\"§´`$%&/()=?\\\täöüß^°-*+,-.,;:<>|@€");
            var uml = builder.Build().AsString();
            Assert.NotEmpty(uml);

            var expectedUml = "@startuml" + Environment.NewLine + "[!\"§´`$%&/()=?\\\täöüß^°-*+,-.,;:<>|@€]" +
                              Environment.NewLine + "[!\"§´`] --> [$%&/()=?]" +
                              Environment.NewLine + "[\\\t%] --> [äöüß]" + Environment.NewLine +
                              "[^°-*+.,;:] --> [<>|@€]" +
                              Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }
    }
}