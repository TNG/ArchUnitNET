//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public interface ICondition<in TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        IEnumerable<ConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        );
        bool CheckEmpty();
    }
}
