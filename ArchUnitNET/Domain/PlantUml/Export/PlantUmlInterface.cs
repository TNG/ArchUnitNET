using System;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlInterface : IPlantUmlElement
    {
        private readonly string _name;
        string _hyperlink { get; }

        public PlantUmlInterface(string name, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var hyperlinkString = _hyperlink != null ? " [[" + _hyperlink + "]] " : null;
            var result =
                "interface \""
                + _name
                + "\""
                + hyperlinkString
                + " {"
                + Environment.NewLine
                + "}"
                + Environment.NewLine;
            return result;
        }
    }
}
