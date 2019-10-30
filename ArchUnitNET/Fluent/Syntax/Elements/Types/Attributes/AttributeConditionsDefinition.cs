//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributeConditionsDefinition
    {
        public static ICondition<Attribute> BeAbstract()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || attribute.IsAbstract.Value, "be abstract",
                "is not abstract");
        }

        public static ICondition<Attribute> BeSealed()
        {
            return new SimpleCondition<Attribute>(attribute => !attribute.IsSealed.HasValue || attribute.IsSealed.Value,
                "be sealed", "is not sealed");
        }


        //Negations


        public static ICondition<Attribute> NotBeAbstract()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || !attribute.IsAbstract.Value, "not be abstract",
                "is abstract");
        }

        public static ICondition<Attribute> NotBeSealed()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsSealed.HasValue || !attribute.IsSealed.Value, "not be sealed", "is sealed");
        }
    }
}