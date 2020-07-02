//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Fluent.Slices
{
    public class GivenSlices
    {
        private readonly SliceRuleCreator _ruleCreator;

        public GivenSlices(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public SlicesShould Should()
        {
            _ruleCreator.AddToDescription("should");
            return new SlicesShould(_ruleCreator);
        }
    }
}