//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

namespace ArchUnitNET.Fluent.PlantUml
{
    public static class PlantUmlDefinition
    {
        public static PlantUmlFluentComponentDiagramInitializer ComponentDiagram()
        {
            var fluentCreator = new PlantUmlFluentComponentDiagramCreator();
            return new PlantUmlFluentComponentDiagramInitializer(fluentCreator);
        }
    }
}
