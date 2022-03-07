using System;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlInterface : IPlantUmlElement
    {
        private readonly string _name;
        string _hyperlink { get; }
        private string _letter;
        private string _letterColor;

        public PlantUmlInterface(string name, string hyperlink = null, string letter = null, string letterColor = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink, letter, letterColor);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
            _letter = letter;
            _letterColor = letterColor;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var attributeString = "";

            if (_letter != null)
            {
                var letterColorString = _letterColor != null ? ',' + _letterColor : "";
                attributeString = $@" << ({_letter}{letterColorString}) >> ";
            }

            var result = "interface " + _name + attributeString + " {" + Environment.NewLine;

            if (_hyperlink != null)
            {
                result += " [[" + _hyperlink + "]] " + Environment.NewLine;
            }

            result += "}" + Environment.NewLine;
            return result;
        }
    }
}