//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.IO;
using System.Text;
using ArchUnitNET.Fluent.Exceptions;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class BuiltPlantUmlFluentComponentDiagram
    {
        private readonly PlantUmlFluentComponentDiagramCreator _fluentComponentDiagramCreator;

        internal BuiltPlantUmlFluentComponentDiagram(
            PlantUmlFluentComponentDiagramCreator fluentComponentDiagramCreator)
        {
            _fluentComponentDiagramCreator = fluentComponentDiagramCreator;
        }

        public string AsString()
        {
            var umlLines = _fluentComponentDiagramCreator.GetBuiltUml();
            var umlSb = new StringBuilder();
            foreach (var line in umlLines)
            {
                umlSb.AppendLine(line);
            }

            return umlSb.ToString();
        }

        public void WriteToFile(string path, bool overwrite = true)
        {
            if (!overwrite && File.Exists(path))
            {
                throw new FileAlreadyExistsException("File already exists and overwriting is disabled.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path."));

            var umlLines = _fluentComponentDiagramCreator.GetBuiltUml();
            using (var sw = File.CreateText(path))
            {
                foreach (var line in umlLines)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}