//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassConditionsDefinition
    {
        public static ICondition<Class> BeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value, "be abstract",
                "is not abstract");
        }

        public static ICondition<Class> BeSealed()
        {
            return new SimpleCondition<Class>(cls => !cls.IsSealed.HasValue || cls.IsSealed.Value, "be sealed",
                "is not sealed");
        }

        public static ICondition<Class> BeValueTypes()
        {
            return new SimpleCondition<Class>(cls => !cls.IsValueType.HasValue || cls.IsValueType.Value,
                "be value types", "is no value type");
        }

        public static ICondition<Class> BeEnums()
        {
            return new SimpleCondition<Class>(cls => !cls.IsEnum.HasValue || cls.IsEnum.Value, "be enums",
                "is no enum");
        }

        public static ICondition<Class> BeStructs()
        {
            return new SimpleCondition<Class>(cls => !cls.IsStruct.HasValue || cls.IsStruct.Value, "be structs",
                "is no struct");
        }


        //Negations


        public static ICondition<Class> NotBeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "not be abstract", "is abstract");
        }

        public static ICondition<Class> NotBeSealed()
        {
            return new SimpleCondition<Class>(cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value, "not be sealed",
                "is sealed");
        }

        public static ICondition<Class> NotBeValueTypes()
        {
            return new SimpleCondition<Class>(cls => !cls.IsValueType.HasValue || !cls.IsValueType.Value,
                "not be value types", "is a value type");
        }

        public static ICondition<Class> NotBeEnums()
        {
            return new SimpleCondition<Class>(cls => !cls.IsEnum.HasValue || !cls.IsEnum.Value, "not be enums",
                "is an enum");
        }

        public static ICondition<Class> NotBeStructs()
        {
            return new SimpleCondition<Class>(cls => !cls.IsStruct.HasValue || !cls.IsStruct.Value, "not be structs",
                "is a struct");
        }
    }
}