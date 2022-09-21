//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class GenerationOptions
    {
        public Func<ITypeDependency, bool> DependencyFilter { get; set; }
        public bool IncludeDependenciesToOther { get; set; }
        public bool IncludeNodesWithoutDependencies { get; set; } = true;
        public bool CompactVersion { get; set; } = false;
        public bool AlternativeView { get; set; } = false;
    }
}