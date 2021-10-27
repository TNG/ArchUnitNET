//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

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
            return _fluentComponentDiagramCreator.Builder.AsString();
        }

        public List<string> AsLineList()
        {
            return _fluentComponentDiagramCreator.Builder.AsLineList();
        }

        public void WriteToFile(string path, bool overwrite = true)
        {
            _fluentComponentDiagramCreator.Builder.WriteToFile(path, overwrite);
        }
    }
}