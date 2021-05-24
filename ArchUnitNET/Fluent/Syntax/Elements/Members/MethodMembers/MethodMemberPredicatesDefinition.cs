﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IPredicate<MethodMember> AreCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return patternList.Any(pattern => ruleType.IsCalledBy(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are called by one of no types (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "are called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreCalledBy(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return AreCalledBy(types);
        }

        public static IPredicate<MethodMember> AreCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return AreCalledBy(types);
        }

        public static IPredicate<MethodMember> AreCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => type.GetCallingTypes().Intersect(types).Any());
            }

            var description = "are called by " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type => type.GetCallingTypes().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type => type.GetCallingTypes().Intersect(iTypeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return patternList.Any(pattern => ruleType.HasDependencyInMethodBodyTo(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(types)
                        .Any());
            }

            var description = "have dependencies in method body to " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(types)
                        .Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(iTypeList)
                        .Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
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

        public static IPredicate<MethodMember> AreNotCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return !patternList.Any(pattern => ruleType.IsCalledBy(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are not called by one of no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "are not called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return AreNotCalledBy(types);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return AreNotCalledBy(types);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => !type.GetCallingTypes().Intersect(types).Any());
            }

            var description = "are not called by " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type => !type.GetCallingTypes().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not called by one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are not called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type => !type.GetCallingTypes().Intersect(iTypeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are not called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimplePredicate<MethodMember>(
                member => !member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "do not have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return !patternList.Any(pattern =>
                    ruleType.HasDependencyInMethodBodyTo(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "do not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "do not have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(IType firstType,
            params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return DoNotHaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(Type firstType,
            params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return DoNotHaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(types)
                        .Any());
            }

            var description = "do not have dependencies in method body to " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(types)
                        .Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "do not have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target).Intersect(iTypeList)
                        .Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "do not have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }
    }
}