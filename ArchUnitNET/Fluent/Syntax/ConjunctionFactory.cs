//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class ConjunctionFactory
    {
        public static TConjunction Create<TConjunction, TRuleType>(
            IArchRuleCreator<TRuleType> ruleCreator
        )
            where TRuleType : ICanBeAnalyzed
        {
            return (TConjunction)Activator.CreateInstance(typeof(TConjunction), ruleCreator);
        }
    }
}
