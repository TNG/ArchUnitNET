//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using System.Linq;

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

        public static ICondition<Class> BeImmutable()
        {
            return new SimpleCondition<Class>(cls => cls.Members.Where(m => m.IsStatic == false).All(m => WriteAccessors.Immutable.HasFlag(m.WriteAccessor)), "be immutable",
                "is not immutable");
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

        public static ICondition<Class> NotBeImmutable()
        {
            return new SimpleCondition<Class>(cls => cls.Members.Any(m => m.IsStatic == false && !WriteAccessors.Immutable.HasFlag(m.WriteAccessor)), "not be immutable",
                "is immutable");
        }
    }
}