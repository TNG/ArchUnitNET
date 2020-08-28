//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit.Sdk;


// ReSharper disable once CheckNamespace
namespace Xunit
{
    partial class Assert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        /// <exception cref="FailedArchRuleException">Thrown if the rule is violated</exception>
        public static void ArchRule(Architecture architecture, IArchRule archRule)
        {
            if (!archRule.HasNoViolations(architecture))
            {
                throw new FailedArchRuleException(architecture, archRule);
            }
        }
    }
}