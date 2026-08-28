using System;
using ArchUnitNET.Domain.PlantUml.Export;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    /// <summary>
    ///     Exercises the <see cref="PlantUmlSlice" /> rendering options that no slice-diagram
    ///     fixture happens to produce: a hyperlink, and a colour on a namespace-less slice.
    /// </summary>
    public class PlantUmlSliceTest
    {
        [Fact]
        public void GetPlantUmlString_WithHyperlink_AppendsHyperlink()
        {
            var slice = new PlantUmlSlice("Slice1", hyperlink: "https://example.com");
            var uml = slice.GetPlantUmlString(new RenderOptions());
            Assert.Equal("[Slice1] [[https://example.com]] " + Environment.NewLine, uml);
        }

        [Fact]
        public void GetPlantUmlString_WithColorAndNoNamespace_AppendsColor()
        {
            var slice = new PlantUmlSlice("Slice1", color: "ff0000");
            var uml = slice.GetPlantUmlString(new RenderOptions());
            Assert.Equal("[Slice1] #ff0000" + Environment.NewLine, uml);
        }
    }
}
