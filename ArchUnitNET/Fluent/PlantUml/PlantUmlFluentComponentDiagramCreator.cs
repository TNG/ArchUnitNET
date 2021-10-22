//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml;

namespace ArchUnitNET.Fluent.PlantUml
{
    internal class PlantUmlFluentComponentDiagramCreator : IHasDescription
    {
        public readonly PlantUmlFileBuilder Builder;
        private List<string> _builtUml;

        public PlantUmlFluentComponentDiagramCreator()
        {
            Description = "Class diagram";
            Builder = new PlantUmlFileBuilder();
        }

        public void AddToDescription(string description)
        {
            Description += " " + description.Trim();
        }

        public void BuildUml()
        {
            _builtUml = Builder.Build();
        }

        public List<string> GetBuiltUml()
        {
            return _builtUml;
        }

        public string Description { get; private set; }
    }
}