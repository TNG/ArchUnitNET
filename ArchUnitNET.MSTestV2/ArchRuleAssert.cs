//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArchUnitNET.MSTestV2
{
    public static class ArchRuleAssert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        public static void FulfilsRule(Architecture architecture, IArchRule archRule)
        {
            if (!archRule.HasNoViolations(architecture))
            {
                Assert.Fail(archRule.Evaluate(architecture).ToErrorMessage());
            }
        }
    }
}
