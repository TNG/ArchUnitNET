//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml;
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
            var uml = builder.Build();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByTypesIncludingDependenciesToOtherTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Types.Take(100), true);
            var uml = builder.Build();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByNamespacesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Architecture.Namespaces);
            var uml = builder.Build();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlBySlicesTest()
        {
            var slices = SliceRuleDefinition.Slices().Matching("ArchUnitNETTests.(*).").GetObjects(Architecture);
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(slices);
            var uml = builder.Build();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByDependenciesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Dependencies);
            var uml = builder.Build();
            Assert.NotEmpty(uml);
        }

        [Fact]
        public void BuildUmlByDependenciesWithObjectsWithNoDependenciesTest()
        {
            var builder = new PlantUmlFileBuilder().WithDependenciesFrom(Dependencies, "d", "");
            var uml = builder.Build();
            Assert.NotEmpty(uml);

            var umlSb = new StringBuilder();
            foreach (var line in uml)
            {
                umlSb.AppendLine(line);
            }

            const string expectedUml = "@startuml\r\n[d]\r\n[a] --> [b]\r\n[b] --> [c]\r\n[c] --> [a]\r\n@enduml\r\n";
            Assert.Equal(expectedUml, umlSb.ToString());
        }
    }
}