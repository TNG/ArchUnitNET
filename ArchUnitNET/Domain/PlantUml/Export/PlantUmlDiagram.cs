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
            var orderedElements = PlantUmlElements
                .OrderBy(element => element.GetType() != typeof(PlantUmlNamespace))
                .ThenBy(element => element.GetType() != typeof(PlantUmlSlice))
                .ThenBy(element => element.GetType() != typeof(PlantUmlClass))
                .ThenBy(element => element.GetType() != typeof(PlantUmlInterface));
            foreach (var element in MergeNestedSlices(orderedElements))
            {
                result.Append(element.GetPlantUmlString(renderOptions));
            }
            result.AppendLine("@enduml");
            return result.ToString();
        }

        /// <summary>
        /// Replaces all nested slices that share a root package with a single tree for that
        /// package, placed where its first slice was. All other elements keep their position.
        /// </summary>
        private static IEnumerable<IPlantUmlElement> MergeNestedSlices(
            IEnumerable<IPlantUmlElement> elements
        )
        {
            var mergedElements = new List<IPlantUmlElement>();
            var sliceTrees = new Dictionary<string, PlantUmlSliceTree>();
            foreach (var element in elements)
            {
                if (!(element is PlantUmlSlice slice) || !slice.IsNested)
                {
                    mergedElements.Add(element);
                }
                else if (sliceTrees.TryGetValue(slice.RootPackage, out var tree))
                {
                    tree.Add(slice);
                }
                else
                {
                    tree = new PlantUmlSliceTree(slice);
                    sliceTrees.Add(slice.RootPackage, tree);
                    mergedElements.Add(tree);
                }
            }

            return mergedElements;
        }
    }
}
