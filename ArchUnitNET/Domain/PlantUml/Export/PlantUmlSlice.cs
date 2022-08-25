﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlSlice : IPlantUmlElement
    {
        private readonly string _name;
        private string _hyperlink { get; }
        private string _namespace { get; }

        public PlantUmlSlice(string name, string nameSpace = null, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink, nameSpace);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
            _namespace = nameSpace;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = "";
            if (_namespace != null)
            {
                result = "package " + _namespace.Remove(_namespace.Length-1) + "{";
                result += Environment.NewLine;
                var name = _name.Remove(0, _namespace.Length);
                int i = 1;
                while (name.Contains("."))
                {
                    var dotPattern = name.IndexOf(".", StringComparison.Ordinal);
                    result += "package " + name.Remove(dotPattern) + "{";
                    result += Environment.NewLine;

                    name = name.Remove(0, dotPattern+1);
                    i++;
                }
                
                result += "[" + name + "] as " + _name;  
                
                while (i>0)
                {
                    result += Environment.NewLine;
                    result += "}";
                    i--;
                }
            }
            else
            {
                result += "[" + _name + "]";
                
            }

            if (_hyperlink != null)
            {
                result += " [[" + _hyperlink + "]] ";
            }

            result += Environment.NewLine;

            return result;
        }
    }
}