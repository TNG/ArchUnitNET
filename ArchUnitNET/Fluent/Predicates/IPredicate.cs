//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    public interface IPredicate<TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        IEnumerable<TRuleType> GetMatchingObjects(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        );
    }
}
