using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlClass : IPlantUmlElement
    {
        private readonly string _name;
        private readonly List<string> _fields = new List<string>();
        string _hyperlink { get; }
        private string _letter;
        private string _letterColor;


        public PlantUmlClass(string name, string hyperlink = null, string letter = null, string letterColor = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink, letter, letterColor);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
            _letter = letter;
            _letterColor = letterColor;
        }

        public void AddField(string field)
        {
            _fields.Add(field);
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var attributeString = "";

            if (_letter != null)
            {
                var letterColorString = _letterColor != null ? ',' + _letterColor : "";
                attributeString = $@" << ({_letter}{letterColorString}) >> ";
            }

            var result = "class " + _name + attributeString + " {" + Environment.NewLine;

            if (_hyperlink != null)
            {
                result += " [[" + _hyperlink + "]] " + Environment.NewLine;
            }

            if (!renderOptions.OmitClassFields)
            {
                result += _fields.Aggregate("", (umlstring, field) => umlstring + field + Environment.NewLine);
            }

            result += "}" + Environment.NewLine;
            return result;
        }
    }
}