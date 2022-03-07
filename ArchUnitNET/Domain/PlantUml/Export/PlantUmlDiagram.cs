using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlDiagram : IPlantUmlContainer
    {
        public List<IPlantUmlElement> PlantUmlElements { get; } = new List<IPlantUmlElement>();

        public void AddElement(IPlantUmlElement plantUmlElement)
        {
            PlantUmlElements.Add(plantUmlElement);
        }

        public void AddElements(IEnumerable<IPlantUmlElement> plantUmlElements)
        {
            PlantUmlElements.AddRange(plantUmlElements);
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = "@startuml" + Environment.NewLine;
            result += PlantUmlElements
                .OrderBy(element => element.GetType() != typeof(PlantUmlNamespace))
                .ThenBy(element => element.GetType() != typeof(PlantUmlSlice))
                .ThenBy(element => element.GetType() != typeof(PlantUmlClass))
                .ThenBy(element => element.GetType() != typeof(PlantUmlInterface))
                .Aggregate("", (umlstring, umlElement) => umlstring + umlElement.GetPlantUmlString(renderOptions));
            result += "@enduml" + Environment.NewLine;
            return result;
        }
    }
}