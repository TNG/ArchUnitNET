//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;

namespace ArchUnitNET.Fluent.Slices
{
    public class SlicesShould
    {
        private readonly SliceRuleCreator _ruleCreator;

        public SlicesShould(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public SliceRule BeFreeOfCycles()
        {
            throw new NotImplementedException();
            return new SliceRule(_ruleCreator);
        }

        public SliceRule NotDependOnEachOther()
        {
            throw new NotImplementedException();
            return new SliceRule(_ruleCreator);
        }
    }
}