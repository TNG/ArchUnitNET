//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
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

        public PlantUmlSlice(string name, string hyperlink = null)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            _hyperlink = hyperlink;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = "[" + _name + "]";

            if (_hyperlink != null)
            {
                result += " [[" + _hyperlink + "]] ";
            }

            result += Environment.NewLine;

            return result;
        }
    }
}