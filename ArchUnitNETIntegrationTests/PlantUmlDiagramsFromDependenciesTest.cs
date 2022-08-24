﻿using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.PlantUml;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.MSTestV2;
using Xunit;

namespace ArchUnitNETIntegrationTests;

public class PlantUmlIntegrationDiagramTests
{
    [Fact]
    public void ComponentDiagramFromSlicesBuild()
    {
        var sliceRule1 = SliceRuleDefinition.Slices().Matching("Microsoft.VisualStudio.TestPlatform.(**).");
        var arch1 = new ArchLoader()
            .LoadAssembly(typeof(Microsoft.VisualStudio.TestPlatform.TestSDKAutoGeneratedCode).Assembly).Build();
        var path1 = "../../../PlantUmlDiagramsFromDependencies/Microsoft.VisualStudio.TestPlatform.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule1, arch1).WriteToFile(path1);
        Assert.True(File.Exists(path1));
            
        var sliceRule2 = SliceRuleDefinition.Slices().Matching("Snapper.(**).");
        var arch2 = new ArchLoader()
            .LoadAssembly(typeof(Snapper.SnapperExtensions).Assembly).Build();
        var path2 = "../../../PlantUmlDiagramsFromDependencies/Snapper.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule2, arch2).WriteToFile(path2);
        Assert.True(File.Exists(path2));
        
        var path3 = "../../../PlantUmlDiagramsFromDependencies/TestPlatform_plus_Snapper.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule1.GetObjects(arch1), sliceRule2.GetObjects(arch2)).WriteToFile(path3);
        Assert.True(File.Exists(path3));
        
        var sliceRule4 = SliceRuleDefinition.Slices().Matching("Microsoft.(**).");
        var arch4 = new ArchLoader()
            .LoadAssembly(typeof(Microsoft.EntityFrameworkCore.CommentAttribute).Assembly).Build();
        var path4 = "../../../PlantUmlDiagramsFromDependencies/Microsoft.EntityFrameworkCore.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule4.GetObjects(arch4)).WriteToFile(path4);
        Assert.True(File.Exists(path4));

        var sliceRule5 = SliceRuleDefinition.Slices().Matching("Google.(**).");
        var arch5 = new ArchLoader()
            .LoadAssembly(typeof(Google.ApplicationContext).Assembly).Build();
        var path5 = "../../../PlantUmlDiagramsFromDependencies/Google.CloudStorage.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule5.GetObjects(arch5)).WriteToFile(path5);
        Assert.True(File.Exists(path5));
        
        var sliceRule6 = SliceRuleDefinition.Slices().Matching("System.(*).");
        var arch6 = new ArchLoader().LoadAssembly(typeof(System.IO.BinaryReader).Assembly).Build();
        var path6 = "../../../PlantUmlDiagramsFromDependencies/SystemIO.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule6.GetObjects(arch6)).WriteToFile(path6);
        Assert.True(File.Exists(path6));
        
        var sliceRule7 = SliceRuleDefinition.Slices().Matching("Newtonsoft.Json.(**).");
        var arch7 = new ArchLoader().LoadAssembly(typeof(Newtonsoft.Json.Formatting).Assembly).Build();
        var path7 = "../../../PlantUmlDiagramsFromDependencies/Newtonsoft.Json.puml";
        PlantUmlDefinition.ComponentDiagram().WithDependenciesFromSlices(sliceRule7.GetObjects(arch7)).WriteToFile(path7);
        Assert.True(File.Exists(path7));
        

    }
}