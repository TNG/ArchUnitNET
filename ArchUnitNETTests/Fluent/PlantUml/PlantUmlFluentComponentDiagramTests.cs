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
using ArchUnitNET.Domain.PlantUml.Export;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Fluent.PlantUml.PlantUmlDefinition;
using Architecture = ArchUnitNET.Domain.Architecture;

namespace ArchUnitNETTests.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(PlantUmlFluentComponentDiagramTests).Assembly).Build();

        private static readonly List<IPlantUmlElement> Dependencies = new List<IPlantUmlElement>
        {
            new PlantUmlDependency("a", "b", DependencyType.OneToOne),
            new PlantUmlDependency("b", "c", DependencyType.Circle),
            new PlantUmlDependency("c", "d", DependencyType.PackageToPackageIfSameParentNamespace),
            new PlantUmlDependency("d", "e", DependencyType.OneToMany),
            new PlantUmlDependency("e", "f", DependencyType.OneToPackage),
            new PlantUmlDependency("f", "g", DependencyType.PackageToOne),
            new PlantUmlDependency("g", "h", DependencyType.PackageToPackage),
            new PlantUmlDependency("h", "i", DependencyType.OneToOneCompact),
            new PlantUmlDependency("i", "a", DependencyType.OneToOneIfSameParentNamespace)
        };

        [Fact]
        public void ComponentDiagramFromSlicesTest()
        {
            var sliceRule = SliceRuleDefinition.Slices().Matching("ArchUnitNETTests.(*).");
            var uml1 = ComponentDiagram().WithDependenciesFromSlices(sliceRule, Architecture).AsString();
            var uml2 = ComponentDiagram().WithDependenciesFromSlices(sliceRule.GetObjects(Architecture)).AsString();
            Assert.NotEmpty(uml1);
            Assert.NotEmpty(uml2);

            var arch1 = new ArchLoader().LoadAssembly(typeof(Architecture).Assembly).Build();
            var sliceRule1 = SliceRuleDefinition.Slices().MatchingWithPackages("ArchUnitNET.(*).(*).(*)");
            const string path = "../../../Fluent/PlantUml/Test.puml";
            var g = new GenerationOptions() { C4Style = true, LimitDependencies = true };
            ComponentDiagram().WithDependenciesFromSlices(sliceRule1.GetObjects(arch1), g).WriteToFile(path);
            File.Delete(path);
        }

        [Fact]
        public void ComponentDiagramFromTypesTest()
        {
            var typeRule = ArchRuleDefinition.Types().That().Are(typeof(PlantUmlFluentComponentDiagramTests));
            var uml1 = ComponentDiagram().WithDependenciesFromTypes(typeRule, Architecture)
                .AsString(new RenderOptions {OmitClassFields = true});
            var uml2 = ComponentDiagram().WithDependenciesFromTypes(typeRule.GetObjects(Architecture))
                .AsString(new RenderOptions {OmitClassFields = true});
            var uml3 = ComponentDiagram()
                .WithDependenciesFromTypes(typeRule, Architecture,
                    new GenerationOptions {IncludeDependenciesToOther = true}).AsString();
            var uml4 = ComponentDiagram().WithDependenciesFromTypes(typeRule.GetObjects(Architecture),
                new GenerationOptions {IncludeDependenciesToOther = true}).AsString();
            Assert.NotEmpty(uml1);
            Assert.NotEmpty(uml2);
            Assert.NotEmpty(uml3);
            Assert.NotEmpty(uml4);
            var expectedUml = "@startuml" + Environment.NewLine + "class \"" +
                              typeof(PlantUmlFluentComponentDiagramTests).FullName +
                              "\" {" + Environment.NewLine + "}" + Environment.NewLine + "@enduml" + Environment.NewLine;
            Assert.Equal(expectedUml, uml1);
            Assert.Equal(expectedUml, uml2);
        }

        [Fact]
        public void ComponentDiagramFromCustomDependenciesTest()
        {
            var classesWithoutDependencies = new[] {new PlantUmlClass("X")};
            var uml = ComponentDiagram().WithElements(Dependencies.Concat(classesWithoutDependencies)).AsString();
            Assert.NotEmpty(uml);

            var expectedUml = "@startuml" + Environment.NewLine + "class \"X\" {" + Environment.NewLine + "}" + 
                              Environment.NewLine + "[a] --|> [b]" +
                              Environment.NewLine + "[b] <-[#red]> [c]" +
                              Environment.NewLine + "c ..> d" +
                              Environment.NewLine + "[d] \"1\" --|> \"many\" [e]" +
                              Environment.NewLine + "[e] -[#red]> f" +
                              Environment.NewLine + "f -[#blue]> [g]" +
                              Environment.NewLine + "g -[#green]> h" +
                              Environment.NewLine + "[h] --> [i]" +
                              Environment.NewLine + "i --|> a" +
                              Environment.NewLine + "@enduml" + 
                              Environment.NewLine;
            Assert.Equal(expectedUml, uml);
        }

        [Fact]
        public void ComponentDiagramWriteToFileTest()
        {
            var classesWithoutDependencies = new[] {new PlantUmlClass("X")};
            const string path = "temp/testUml.puml";
            var expectedUml = new[]
                {"@startuml", "class \"X\" {", "}", 
                    "[a] --|> [b]",
                    "[b] <-[#red]> [c]",
                    "c ..> d",
                    "[d] \"1\" --|> \"many\" [e]",
                    "[e] -[#red]> f",
                    "f -[#blue]> [g]",
                    "g -[#green]> h",
                    "[h] --> [i]",
                    "i --|> a",
                    "@enduml"};
            ComponentDiagram().WithElements(Dependencies.Concat(classesWithoutDependencies)).WriteToFile(path);
            Assert.True(File.Exists(path));

            using (var sr = File.OpenText(path))
            {
                var i = 0;
                var s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Assert.Equal(expectedUml[i], s);
                    i++;
                }
            }

            File.Delete(path);

            if (!Directory.EnumerateFileSystemEntries("temp").Any())
            {
                Directory.Delete("temp");
            }
        }
    }
}