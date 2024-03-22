//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain
{
    public class AttributeInstance : TypeInstance<Attribute>
    {
        // ReSharper disable SuggestBaseTypeForParameter
        public AttributeInstance(
            Attribute attribute,
            IEnumerable<AttributeArgument> attributeArguments
        )
            : base(attribute)
        {
            AttributeArguments = attributeArguments;
        }

        public AttributeInstance(Attribute attribute)
            : base(attribute)
        {
            AttributeArguments = Enumerable.Empty<AttributeArgument>();
        }

        public IEnumerable<AttributeArgument> AttributeArguments { get; }

        public bool HasAttributeArguments => AttributeArguments.Any();
    }
}
