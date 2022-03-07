//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    internal class PlantUmlFluentComponentDiagramCreator : IHasDescription
    {
        public readonly PlantUmlFileBuilder Builder;

        public PlantUmlFluentComponentDiagramCreator()
        {
            Description = "Class diagram";
            Builder = new PlantUmlFileBuilder();
        }

        public void AddToDescription(string description)
        {
            Description += " " + description.Trim();
        }

        public string Description { get; private set; }
    }
}