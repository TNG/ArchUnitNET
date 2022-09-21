using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlNamespace : IPlantUmlContainer
    {
        public string Name { get; }
        public List<IPlantUmlElement> PlantUmlElements { get; } = new List<IPlantUmlElement>();
        string _hyperlink { get; }

        public PlantUmlNamespace(string name, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            Name = name;
            _hyperlink = hyperlink;
        }

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
            var hyperlinkString = _hyperlink != null ? " [[" + _hyperlink + "]] " : null;
            var result = "namespace " + Name + hyperlinkString + " {" + Environment.NewLine;

            result += PlantUmlElements.Aggregate("", (umlString, umlElement) =>
                umlString + umlElement.GetPlantUmlString(renderOptions));
            result += "}" + Environment.NewLine;
            return result;
        }
    }
}