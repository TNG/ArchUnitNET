//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using System.Linq;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassPredicatesDefinition
    {
        public static IPredicate<Class> AreAbstract()
        {
            return new SimplePredicate<Class>(cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value, "are abstract");
        }

        public static IPredicate<Class> AreSealed()
        {
            return new SimplePredicate<Class>(cls => !cls.IsSealed.HasValue || cls.IsSealed.Value, "are sealed");
        }

        public static IPredicate<Class> AreImmutable()
        {
            return new SimplePredicate<Class>(cls => cls.Members.All(x => x.IsImmutable != false), "are immutable");
        }

        //Negations

        public static IPredicate<Class> AreNotAbstract()
        {
            return new SimplePredicate<Class>(cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "are not abstract");
        }

        public static IPredicate<Class> AreNotSealed()
        {
            return new SimplePredicate<Class>(cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value, "are not sealed");
        }

        public static IPredicate<Class> AreNotImmutable()
        {
            return new SimplePredicate<Class>(cls => cls.Members.Any(x => x.IsImmutable == false), "are not immutable");
        }
    }
}