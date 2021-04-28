//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Fluent.Freeze
{
    public interface IViolationStore
    {
        bool RuleAlreadyFrozen(IArchRule rule);
        IEnumerable<StringIdentifier> GetFrozenViolations(IArchRule rule);
        void StoreCurrentViolations(IArchRule rule, IEnumerable<StringIdentifier> violations);
    }
}