//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Fluent.PlantUml
{
    public class GivenDefinedPlantUmlFluentComponentDiagram
    {
        private readonly PlantUmlFluentComponentDiagramCreator _fluentComponentDiagramCreator;

        internal GivenDefinedPlantUmlFluentComponentDiagram(
            PlantUmlFluentComponentDiagramCreator fluentComponentDiagramCreator)
        {
            _fluentComponentDiagramCreator = fluentComponentDiagramCreator;
        }

        public BuiltPlantUmlFluentComponentDiagram Build()
        {
            _fluentComponentDiagramCreator.BuildUml();
            return new BuiltPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
    }
}