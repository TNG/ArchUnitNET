//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Fluent.PlantUml.PlantUmlDefinition;

namespace ArchUnitNETTests.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(PlantUmlFluentComponentDiagramTests).Assembly).Build();

        private static readonly List<PlantUmlDependency> Dependencies = new List<PlantUmlDependency>
        {
            new PlantUmlDependency("a", "b"),
            new PlantUmlDependency("b", "c"),
            new PlantUmlDependency("c", "a")
        };

        [Fact]
        public void ComponentDiagramFromSlicesTest()
        {
            var sliceRule = SliceRuleDefinition.Slices().Matching("ArchUnitNETTests.(*).");
            var uml1 = ComponentDiagram().WithDependenciesFromSlices(sliceRule, Architecture).Build().AsString();
            var uml2 = ComponentDiagram().WithDependenciesFromSlices(sliceRule.GetObjects(Architecture)).Build()
                .AsString();
            Assert.NotEmpty(uml1);
            Assert.NotEmpty(uml2);
        }

        [Fact]
        public void ComponentDiagramFromTypesTest()
        {
            var typeRule = ArchRuleDefinition.Types().That().Are(typeof(PlantUmlFluentComponentDiagramTests));
            var uml1 = ComponentDiagram().WithDependenciesFromTypes(typeRule, Architecture).Build().AsString();
            var uml2 = ComponentDiagram().WithDependenciesFromTypes(typeRule.GetObjects(Architecture)).Build()
                .AsString();
            var uml3 = ComponentDiagram().WithDependenciesFromTypes(typeRule, Architecture, true).Build().AsString();
            var uml4 = ComponentDiagram().WithDependenciesFromTypes(typeRule.GetObjects(Architecture), true).Build()
                .AsString();
            Assert.NotEmpty(uml1);
            Assert.NotEmpty(uml2);
            Assert.NotEmpty(uml3);
            Assert.NotEmpty(uml4);

            var expectedUml = "@startuml" + Environment.NewLine + "[" +
                              typeof(PlantUmlFluentComponentDiagramTests).FullName +
                              "]" + Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml1);
            Assert.Equal(expectedUml, uml2);
        }

        [Fact]
        public void ComponentDiagramFromCustomDependenciesTest()
        {
            var uml1 = ComponentDiagram().WithCustomDependencies(Dependencies, "d", "").Build().AsString();
            var uml2 = ComponentDiagram().WithCustomDependencies(Dependencies, new[] { "", "d" }).Build().AsString();
            Assert.NotEmpty(uml1);
            Assert.NotEmpty(uml2);

            var expectedUml = "@startuml" + Environment.NewLine + "[d]" + Environment.NewLine + "[a] --> [b]" +
                              Environment.NewLine + "[b] --> [c]" + Environment.NewLine + "[c] --> [a]" +
                              Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml1);
            Assert.Equal(expectedUml, uml2);
        }

        [Fact]
        public void ComponentDiagramWriteToFileTest()
        {
            const string path1 = "temp/testUml1.puml";
            const string path2 = "temp/testUml2.puml";
            var expectedUml = new[]
                { "@startuml", "[d]", "[a] --> [b]", "[b] --> [c]", "[c] --> [a]", "@enduml" };
            ComponentDiagram().WithCustomDependencies(Dependencies, "d", "").Build().WriteToFile(path1);
            ComponentDiagram().WithCustomDependencies(Dependencies, new[] { "", "d" }).Build().WriteToFile(path2);
            Assert.True(File.Exists(path1));
            Assert.True(File.Exists(path2));

            using (var sr = File.OpenText(path1))
            {
                var i = 0;
                var s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Assert.Equal(expectedUml[i], s);
                    i++;
                }
            }

            using (var sr = File.OpenText(path2))
            {
                var i = 0;
                var s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Assert.Equal(expectedUml[i], s);
                    i++;
                }
            }

            File.Delete(path1);
            File.Delete(path2);

            if (!Directory.EnumerateFileSystemEntries("temp").Any())
            {
                Directory.Delete("temp");
            }
        }
    }
}