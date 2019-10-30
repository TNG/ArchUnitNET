//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.NUnit
{
    public static class ArchRuleExtensions
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="archRule">The rule to test the architecture with</param>
        /// <param name="architecture">The architecture to be tested</param>
        public static void Check(this IArchRule archRule, Architecture architecture)
        {
            ArchRuleAssert.FulfilsRule(architecture, archRule);
        }

        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        public static void CheckRule(this Architecture architecture, IArchRule archRule)
        {
            ArchRuleAssert.FulfilsRule(architecture, archRule);
        }
    }
}