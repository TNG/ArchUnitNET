using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypePredicatesDefinition<T> where T : IType
    {
        public static IPredicate<T> Are(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return architecture.GetITypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> Are(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Any(type => type.Equals(ruleType));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are \"" + firstType.FullName + "\"", (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            var description = "are assignable to types with full name " +
                              (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"";
            return new SimplePredicate<T>(type => type.IsAssignableTo(pattern, useRegularExpressions), description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(T ruleType)
            {
                return patternList.Any(pattern => ruleType.IsAssignableTo(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "are assignable to types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.IsAssignableTo(firstType) || moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return ruleType.IsAssignableTo(architecture.GetITypeOfType(firstType)) ||
                       moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetITypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return typeList.Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> ImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => type.ImplementsInterface(pattern, useRegularExpressions),
                "implement interface with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> ResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => type.ResidesInNamespace(pattern, useRegularExpressions),
                "reside in namespace with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> ResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => type.ResidesInAssembly(pattern, useRegularExpressions),
                "reside in assembly with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static IPredicate<T> ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            bool Condition(T type, Architecture architecture)
            {
                return type.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly)) ||
                       moreAssemblies.Any(asm => type.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm)));
            }

            var description = moreAssemblies.Aggregate("reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\"");

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> HavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
        }

        public static IPredicate<T> HaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNested()
        {
            return new SimplePredicate<T>(type => type.IsNested, "are nested");
        }


        //Negations


        public static IPredicate<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return !architecture.GetITypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are not \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNot(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).All(type => !type.Equals(ruleType));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are not \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNotAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            var description = "are not assignable to types with full name " +
                              (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"";
            return new SimplePredicate<T>(type => !type.IsAssignableTo(pattern, useRegularExpressions), description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(T ruleType)
            {
                return patternList.All(pattern => !ruleType.IsAssignableTo(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "are not assignable to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.IsAssignableTo(firstType) && !moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !ruleType.IsAssignableTo(architecture.GetITypeOfType(firstType)) &&
                       !moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetITypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are not assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return !typeList.Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetITypeOfType).Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }


        public static IPredicate<T> DoNotImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => !type.ImplementsInterface(pattern, useRegularExpressions),
                "do not implement interface with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => !type.ResidesInNamespace(pattern, useRegularExpressions),
                "do not reside in namespace with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            return new SimplePredicate<T>(type => !type.ResidesInAssembly(pattern, useRegularExpressions),
                "do not reside in assembly with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            bool Condition(T type, Architecture architecture)
            {
                return !type.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly)) &&
                       !moreAssemblies.Any(asm => type.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm)));
            }

            var description = moreAssemblies.Aggregate("do not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\"");

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNotNested()
        {
            return new SimplePredicate<T>(type => !type.IsNested, "are not nested");
        }
    }
}