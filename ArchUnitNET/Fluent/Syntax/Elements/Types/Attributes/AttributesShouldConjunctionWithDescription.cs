//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<AttributesShould, Attribute>
    {
        public AttributesShouldConjunctionWithDescription(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }
    }
}
