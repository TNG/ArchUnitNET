//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace ArchUnitNET.xUnit
{
    public class FailedArchRuleException : XunitException
    {
        /// <summary>
        ///     Creates a new instance of the <see href="FailedArchRuleException" /> class.
        /// </summary>
        /// <param name="architecture">The architecture which was tested</param>
        /// <param name="archRule">The archrule that failed</param>
        public FailedArchRuleException(Architecture architecture, IArchRule archRule)
            : this(archRule.Evaluate(architecture))
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see href="FailedArchRuleException" /> class.
        /// </summary>
        /// <param name="evaluationResults">The results of the evaluation of the archrule</param>
        public FailedArchRuleException(IEnumerable<EvaluationResult> evaluationResults)
            : base(evaluationResults.ToErrorMessage())
        {
        }
    }
}