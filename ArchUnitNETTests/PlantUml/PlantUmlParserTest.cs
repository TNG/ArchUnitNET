using ArchUnitNET.PlantUml;
using ArchUnitNETTests.PlantUml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ArchUnitNETTests
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

            Assert.Equal(2, diagram.PlantUmlComponents.Count);
        }

        private PlantUmlDiagram CreateDiagram(string path)
        {
            return parser.Parse(path);
        }
    }
}
