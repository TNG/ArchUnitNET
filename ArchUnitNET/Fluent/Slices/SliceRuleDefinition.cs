//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Fluent.Slices
{
    public static class SliceRuleDefinition
    {
        public static SliceRuleInitializer Slices()
        {
            var ruleCreator = new SliceRuleCreator();
            return new SliceRuleInitializer(ruleCreator);
        }
    }
}