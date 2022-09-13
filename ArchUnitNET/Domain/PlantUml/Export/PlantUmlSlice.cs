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
            // var sb = new StringBuilder();
            // sb.Append("package");
            // sb.AppendFormat("package {0} {{", Namespace.Remove(Namespace.Length - 1));
            // return sb.ToString();
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
                    result.Append("[" + name + "] as " + _name) ;
                    if (Color != null)
                    {
                        result.AppendLine(" #" + Color) ;
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

            if (Hyperlink != null)
            {
                result.Append(" [[" + Hyperlink + "]] ");
            }

            return result.ToString() + Environment.NewLine;
        }
    }
}