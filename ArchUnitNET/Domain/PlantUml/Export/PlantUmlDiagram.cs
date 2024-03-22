using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var result = new StringBuilder();
            result.AppendLine("@startuml").AppendLine();
            result
                .AppendLine(
                    "!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml"
                )
                .AppendLine();
            result.AppendLine("HIDE_STEREOTYPE()").AppendLine();
            result.Append(
                PlantUmlElements
                    .OrderBy(element => element.GetType() != typeof(PlantUmlNamespace))
                    .ThenBy(element => element.GetType() != typeof(PlantUmlSlice))
                    .ThenBy(element => element.GetType() != typeof(PlantUmlClass))
                    .ThenBy(element => element.GetType() != typeof(PlantUmlInterface))
                    .Aggregate(
                        "",
                        (umlString, umlElement) =>
                            umlString + umlElement.GetPlantUmlString(renderOptions)
                    )
            );
            result.AppendLine("@enduml");
            return result.ToString();
        }
    }
}
