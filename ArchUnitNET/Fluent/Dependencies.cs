// Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNET.Fluent
{
    public static class Dependencies
    {
        /// <summary>
        /// Verifies that a source assembly only depends on a set of target assemblies.
        /// </summary>
        /// <param name="source">The source assembly</param>
        /// <param name="target">The assemblies which are allowed to be used from the source assembly</param>
        public static IArchRule Check(Assembly source, IEnumerable<Assembly> target)
        {
            var allowedTypes = Types().That().ResideInAssembly(source);
            allowedTypes =
                target.Aggregate(allowedTypes, (current, assembly) => current.Or().ResideInAssembly(assembly));

            var rule = Types()
                .That()
                .ResideInAssembly(source)
                .Should()
                .OnlyDependOn(allowedTypes);

            return rule;
        }
    }
}