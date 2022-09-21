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
            var result = "@startuml" + Environment.NewLine + Environment.NewLine
                                     + "!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml" 
                                     + Environment.NewLine + Environment.NewLine + "HIDE_STEREOTYPE()" + Environment.NewLine + Environment.NewLine;
            result += PlantUmlElements
                .OrderBy(element => element.GetType() != typeof(PlantUmlNamespace))
                .ThenBy(element => element.GetType() != typeof(PlantUmlSlice))
                .ThenBy(element => element.GetType() != typeof(PlantUmlClass))
                .ThenBy(element => element.GetType() != typeof(PlantUmlInterface))
                .Aggregate("", (umlString, umlElement) => umlString + umlElement.GetPlantUmlString(renderOptions));
            result += "@enduml" + Environment.NewLine;
            return result;
        }
    }
}