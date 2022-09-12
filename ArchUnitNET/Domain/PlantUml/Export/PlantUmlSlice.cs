//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlSlice : IPlantUmlElement
    {
        private readonly string _name;
        private string Hyperlink { get; }
        private string Namespace { get; }
        private string Color { get; }
        public PlantUmlSlice(string name, string nameSpace = null, string color = null, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink, nameSpace);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            Hyperlink = hyperlink;
            Namespace = nameSpace;
            Color = color;
        }

        public override string ToString()
        {
            return _name;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = "";
            if (Namespace != null)
            {
                result = "package " + Namespace.Remove(Namespace.Length - 1) + "{";
                result += Environment.NewLine;
                var name = _name.Remove(0, Namespace.Length);
                var i = 1;
                while (name.Contains("."))
                {
                    var dotPattern = name.IndexOf(".", StringComparison.Ordinal);
                    result += "package " + name.Remove(dotPattern) + "{";
                    result += Environment.NewLine;

                    name = name.Remove(0, dotPattern + 1);
                    i++;
                }

                if (name != "")
                {
                    result += "[" + name + "] as " + _name;
                    if (Color != null)
                    {
                        result += " #" + Color;
                    }
                }
                else if (Color != null)
                {
                    result = result.Remove(result.LastIndexOf("{", StringComparison.Ordinal));
                    result += " #" + Color + "{" + Environment.NewLine;
                }

                while (i > 0)
                {
                    result += Environment.NewLine;
                    result += "}";
                    i--;
                }
            }
            else
            {
                result += "[" + _name + "]";
                if (Color != null)
                {
                    result += " #" + Color;
                }
            }

            if (Hyperlink != null)
            {
                result += " [[" + Hyperlink + "]] ";
            }

            return result + Environment.NewLine;
        }
    }
}