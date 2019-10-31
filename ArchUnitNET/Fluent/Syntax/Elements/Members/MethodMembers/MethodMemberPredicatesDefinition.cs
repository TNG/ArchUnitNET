//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberPredicatesDefinition
    {
        public static IPredicate<MethodMember> AreConstructors()
        {
            return new SimplePredicate<MethodMember>(member => member.IsConstructor(), "are constructors");
        }

        public static IPredicate<MethodMember> AreVirtual()
        {
            return new SimplePredicate<MethodMember>(member => member.IsVirtual, "are virtual");
        }

        public static IPredicate<MethodMember> AreCalledBy(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => member.IsCalledBy(pattern, useRegularExpressions),
                "are called by types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }


        //Negations


        public static IPredicate<MethodMember> AreNoConstructors()
        {
            return new SimplePredicate<MethodMember>(member => !member.IsConstructor(), "are no constructors");
        }

        public static IPredicate<MethodMember> AreNotVirtual()
        {
            return new SimplePredicate<MethodMember>(member => !member.IsVirtual, "are not virtual");
        }

        public static IPredicate<MethodMember> AreNotCalledBy(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => !member.IsCalledBy(pattern, useRegularExpressions),
                "are not called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" +
                pattern + "\"");
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => !member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "do not have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }
    }
}