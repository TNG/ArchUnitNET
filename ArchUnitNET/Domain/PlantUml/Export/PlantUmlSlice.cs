//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlSlice : IPlantUmlElement
    {
        private readonly string _name;
        private string Hyperlink { get; }
        private string Namespace { get; }
        private string Color { get; }
        private bool C4Style { get; set; } = false;

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

        public void ChangeView()
        {
            C4Style = !C4Style;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = C4Style ? BuildStringAlternative() : BuildString();

            if (Hyperlink != null)
            {
                result.Append(" [[" + Hyperlink + "]] ");
            }

            return result.ToString() + Environment.NewLine;
        }

        private StringBuilder BuildString()
        {
            var result = new StringBuilder();
            if (Namespace != null)
            {
                result.Append("package " + Namespace.Remove(Namespace.Length - 1));
                var name = _name.Remove(0, Namespace.Length);
                var iter = 1;
                while (name.Contains("."))
                {
                    var dotPattern = name.IndexOf(".", StringComparison.Ordinal);
                    result.AppendLine(" {");
                    result.Append("package " + name.Remove(dotPattern));
                    name = name.Remove(0, dotPattern + 1);
                    iter++;
                }

                if (name != "")
                {
                    result.AppendLine(" {");
                    result.Append("[" + name + "] as " + _name);
                    if (Color != null)
                    {
                        result.AppendLine(" #" + Color);
                    }
                    else
                    {
                        result.AppendLine();
                    }
                }
                else if (Color != null)
                {
                    result.AppendLine(" #" + Color + " {");
                }
                else
                {
                    result.AppendLine(" {");
                }

                for (var i = iter; i > 0; i--)
                {
                    result.AppendLine("}");
                }
            }
            else
            {
                result.Append("[" + _name + "]");
                if (Color != null)
                {
                    result.Append(" #" + Color);
                }
            }

            return result;
        }

        private StringBuilder BuildStringAlternative()
        {
            var result = new StringBuilder();
            if (Namespace != null)
            {
                result.Append("Boundary(" + Namespace.Remove(Namespace.Length - 1) + ", " + Namespace.Remove(Namespace.Length - 1) + ") ");
                var name = _name.Remove(0, Namespace.Length);
                var iter = 1;
                while (name.Contains("."))
                {
                    var dotPattern = name.IndexOf(".", StringComparison.Ordinal);
                    result.AppendLine(" {");
                    result.Append("Boundary(" + name.Remove(dotPattern) + ", " + name.Remove(dotPattern) + ") ");
                    name = name.Remove(0, dotPattern + 1);
                    iter++;
                }
                result.AppendLine(" {");
                if (name != "")
                {
                    result.Append("Container("+ _name + ", " + name + ")");
                    result.AppendLine();
                    
                }
                
                for (var i = iter; i > 0; i--)
                {
                    result.AppendLine("}");
                }
            }
            else
            {
                result.Append("Container(" + _name +", " + _name + ")");
            }

            return result;
        }
    }
}