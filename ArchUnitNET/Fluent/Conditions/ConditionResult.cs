//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ConditionResult
    {
        public readonly ICanBeAnalyzed AnalyzedObject;
        [CanBeNull] public readonly string FailDescription;
        public readonly bool Pass;

        public ConditionResult(ICanBeAnalyzed analyzedObject, bool pass, string failDescription = null)
        {
            Pass = pass;
            AnalyzedObject = analyzedObject;
            FailDescription = pass ? null : failDescription;
        }
    }
}