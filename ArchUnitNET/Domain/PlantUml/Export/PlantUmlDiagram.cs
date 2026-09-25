using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            AssignAliases(PlantUmlElements);
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

        // Letters, digits, "_" and "." are what PlantUML reliably accepts in an id, both after
        // "as" and in a C4 macro.
        private static readonly Regex CharactersNotAllowedInId = new Regex(
            @"[^\p{L}\p{Nd}_.]",
            RegexOptions.Compiled
        );

        /// <summary>
        /// Gives every slice whose name PlantUML would not accept as an id a replacement id, and
        /// points the dependencies at it.
        /// </summary>
        /// <remarks>
        /// A slice is identified in the diagram by its name, but not every name is a valid id.
        /// PlantUML rejects "[Inner] as App.Outer+Inner" and "Container(App.Orders.*.Http, ...)"
        /// as syntax errors, and slice names can contain such characters: SlicedBy names slices
        /// after type names such as "Outer+Inner" or "List`1", and a hand-built SliceIdentifier
        /// can be called anything. No character that a namespace cannot contain works in their
        /// place either, so the id is made from the name with those characters replaced by "_",
        /// and suffixed until it clashes with no other name in the diagram. The name itself stays
        /// the label, and slices whose names are valid ids keep them.
        /// </remarks>
        private static void AssignAliases(IReadOnlyCollection<IPlantUmlElement> elements)
        {
            var slices = elements.OfType<PlantUmlSlice>().ToList();
            var dependencies = elements.OfType<PlantUmlDependency>().ToList();
            // Only slices are declared with an id; a dependency between components that are not
            // declared names them in brackets, where PlantUML accepts more.
            var sliceNames = slices.Select(slice => slice.ToString()).Distinct().ToList();
            var taken = new HashSet<string>(
                sliceNames.Concat(dependencies.SelectMany(dep => new[] { dep.Origin, dep.Target }))
            );

            // In the order the slices appear, so the same diagram always gets the same ids.
            var aliases = new Dictionary<string, string>();
            foreach (var name in sliceNames.Where(name => CharactersNotAllowedInId.IsMatch(name)))
            {
                var alias = CharactersNotAllowedInId.Replace(name, "_");
                var candidate = alias;
                for (var suffix = 2; taken.Contains(candidate); suffix++)
                {
                    candidate = alias + "_" + suffix;
                }

                taken.Add(candidate);
                aliases.Add(name, candidate);
            }

            foreach (var slice in slices)
            {
                slice.Alias = AliasOf(slice.ToString());
            }

            foreach (var dependency in dependencies)
            {
                dependency.OriginAlias = AliasOf(dependency.Origin);
                dependency.TargetAlias = AliasOf(dependency.Target);
            }

            string AliasOf(string name) => aliases.TryGetValue(name, out var alias) ? alias : name;
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
