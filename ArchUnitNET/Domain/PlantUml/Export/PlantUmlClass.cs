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

        public PlantUmlClass(string name, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
        }

        public void AddField(string field)
        {
            _fields.Add(field);
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var hyperlinkString = _hyperlink != null ? " [[" + _hyperlink + "]] " : null;
            var result = "class \"" + _name + "\"" + hyperlinkString + " {" + Environment.NewLine;

            if (!renderOptions.OmitClassFields)
            {
                result += _fields.Aggregate(
                    "",
                    (umlstring, field) => umlstring + "\"" + field + "\"" + Environment.NewLine
                );
            }

            result += "}" + Environment.NewLine;
            return result;
        }
    }
}
