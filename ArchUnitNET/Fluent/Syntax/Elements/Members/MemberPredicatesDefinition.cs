﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberPredicatesDefinition<T> where T : IMember
    {
        public static IPredicate<T> AreDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(member => member.IsDeclaredIn(pattern, useRegularExpressions),
                "are declared in types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(T ruleType)
            {
                return patternList.Any(pattern => ruleType.IsDeclaredIn(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(obj => !obj.Equals(firstPattern)).Distinct().Aggregate(
                    "are declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> members)
            {
                var types = moreTypes.Concat(new[] {firstType});
                return members.Intersect(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = moreTypes.Aggregate("are declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var types = moreTypes.Concat(new[] {firstType}).Select(architecture.GetITypeOfType);
                return members.Intersect(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = moreTypes.Aggregate("are declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return members.Intersect(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = "are declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members)
            {
                return members.Intersect(typeList.SelectMany(type => type.Members).OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "are declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var tList = typeList.Select(architecture.GetITypeOfType);
                return members.Intersect(tList.SelectMany(type => type.Members).OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }


        //Negations


        public static IPredicate<T> AreNotDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(member => !member.IsDeclaredIn(pattern, useRegularExpressions),
                "are not declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(T ruleType)
            {
                return patternList.All(pattern => !ruleType.IsDeclaredIn(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are not declared in no type (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(obj => !obj.Equals(firstPattern)).Distinct().Aggregate(
                    "are not declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> members)
            {
                var types = moreTypes.Concat(new[] {firstType});
                return members.Except(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = moreTypes.Aggregate("are not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var types = moreTypes.Concat(new[] {firstType}).Select(architecture.GetITypeOfType);
                return members.Except(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = moreTypes.Aggregate("are not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return members.Except(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = "are not declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members)
            {
                return members.Except(typeList.SelectMany(type => type.Members).OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "are not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var tList = typeList.Select(architecture.GetITypeOfType);
                return members.Except(tList.SelectMany(type => type.Members).OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in any type (always true9";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }
    }
}