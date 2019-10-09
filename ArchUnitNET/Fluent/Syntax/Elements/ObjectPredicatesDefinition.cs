using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public static class ObjectPredicatesDefinition<T> where T : ICanBeAnalyzed
    {
        public static IPredicate<T> Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var description = moreObjects.Aggregate("are \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new Predicate<T>(o => o.Equals(firstObject) || moreObjects.Any(o.Equals), description);
        }

        public static IPredicate<T> Are(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "do not exist (always empty)";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "are \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new Predicate<T>(obj => objectList.Any(o => o.Equals(obj)), description);
        }

        public static IPredicate<T> DependOnAnyTypesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(obj => obj.DependsOn(pattern),
                "depend on any types with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DependOnAnyTypesWithFullNameContaining(string pattern)
        {
            return new Predicate<T>(obj => obj.FullNameContains(pattern),
                "depend on any types with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DependOnAny(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T type)
            {
                return type.GetTypeDependencies().Any(target => target.Equals(firstType) || moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new Predicate<T>(Condition, description);
        }

        public static IPredicate<T> DependOnAny(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T type, Architecture architecture)
            {
                return type.GetTypeDependencies().Any(target =>
                    target.Equals(architecture.GetTypeOfType(firstType)) ||
                    moreTypes.Any(t => architecture.GetTypeOfType(t).Equals(target)));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            bool Filter(T type, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return type.GetTypeDependencies().Any(target => types.Contains(target));
            }

            var description = "depend on any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().Any(target => typeList.Any(t => t.Equals(target)));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new Predicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T type, Architecture architecture)
            {
                return type.GetTypeDependencies().Any(target =>
                    typeList.Select(architecture.GetTypeOfType).Any(t => t.Equals(target)));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOnTypesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(obj => obj.OnlyDependsOn(pattern),
                "only depend on types with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            bool Filter(T type)
            {
                return type.GetTypeDependencies().All(target => target.Equals(firstType) || moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new Predicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T type, Architecture architecture)
            {
                return type.GetTypeDependencies().All(target =>
                    target.Equals(architecture.GetTypeOfType(firstType)) ||
                    moreTypes.Any(t => architecture.GetTypeOfType(t).Equals(target)));
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            bool Filter(T type, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return type.GetTypeDependencies().All(target => types.Contains(target));
            }

            var description = "only depend on " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().All(target => typeList.Contains(target));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new Predicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T type, Architecture architecture)
            {
                return type.GetTypeDependencies()
                    .All(target => typeList.Select(architecture.GetTypeOfType).Contains(target));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveName(string name)
        {
            return new Predicate<T>(obj => obj.Name.Equals(name), "have name \"" + name + "\"");
        }

        public static IPredicate<T> HaveNameMatching(string pattern)
        {
            return new Predicate<T>(obj => obj.NameMatches(pattern), "have name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveFullName(string fullname)
        {
            return new Predicate<T>(obj => obj.FullName.Equals(fullname), "have full name \"" + fullname + "\"");
        }

        public static IPredicate<T> HaveFullNameMatching(string pattern)
        {
            return new Predicate<T>(obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameStartingWith(string pattern)
        {
            return new Predicate<T>(obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameEndingWith(string pattern)
        {
            return new Predicate<T>(obj => obj.NameEndsWith(pattern), "have name ending with \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveNameContaining(string pattern)
        {
            return new Predicate<T>(obj => obj.NameContains(pattern), "have name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> ArePrivate()
        {
            return new Predicate<T>(obj => obj.Visibility == Private, "are private");
        }

        public static IPredicate<T> ArePublic()
        {
            return new Predicate<T>(obj => obj.Visibility == Public, "are public");
        }

        public static IPredicate<T> AreProtected()
        {
            return new Predicate<T>(obj => obj.Visibility == Protected, "are protected");
        }

        public static IPredicate<T> AreInternal()
        {
            return new Predicate<T>(obj => obj.Visibility == Internal, "are internal");
        }

        public static IPredicate<T> AreProtectedInternal()
        {
            return new Predicate<T>(obj => obj.Visibility == ProtectedInternal, "are protected internal");
        }

        public static IPredicate<T> ArePrivateProtected()
        {
            return new Predicate<T>(obj => obj.Visibility == PrivateProtected, "are private protected");
        }


        //Negations


        public static IPredicate<T> AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var description = moreObjects.Aggregate("are not \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new Predicate<T>(o => !o.Equals(firstObject) && !moreObjects.Any(o.Equals), description);
        }

        public static IPredicate<T> AreNot(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "are not no object (always true)";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "are not \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new Predicate<T>(obj => objectList.All(o => !o.Equals(obj)), description);
        }

        public static IPredicate<T> DoNotDependOnAnyTypesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(obj => !obj.DependsOn(pattern),
                "do not depend on any types with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotDependOnAnyTypesWithFullNameContaining(string pattern)
        {
            return new Predicate<T>(obj => !obj.FullNameContains(pattern),
                "do not depend on any types with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            bool Filter(T type)
            {
                return type.GetTypeDependencies()
                    .All(target => !target.Equals(firstType) && !moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("do not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new Predicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T type, Architecture architecture)
            {
                return type.GetTypeDependencies().All(target =>
                    !target.Equals(architecture.GetTypeOfType(firstType)) &&
                    moreTypes.All(t => !architecture.GetTypeOfType(t).Equals(target)));
            }

            var description = moreTypes.Aggregate("do not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            bool Filter(T type, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return type.GetTypeDependencies().All(target => types.All(t => !t.Equals(target)));
            }

            var description = "do not depend on any " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Filter(T type)
            {
                return type.GetTypeDependencies().All(target => typeList.All(t => !t.Equals(target)));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "do not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new Predicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T type, Architecture architecture)
            {
                return type.GetTypeDependencies().All(target =>
                    typeList.Select(architecture.GetTypeOfType).All(t => !t.Equals(target)));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "do not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveName(string name)
        {
            return new Predicate<T>(obj => !obj.Name.Equals(name), "do not have name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveNameMatching(string pattern)
        {
            return new Predicate<T>(obj => !obj.NameMatches(pattern),
                "do not have name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveFullName(string fullname)
        {
            return new Predicate<T>(obj => !obj.FullName.Equals(fullname),
                "do not have full name \"" + fullname + "\"");
        }

        public static IPredicate<T> DoNotHaveFullNameMatching(string pattern)
        {
            return new Predicate<T>(obj => !obj.FullNameMatches(pattern),
                "do not have full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameStartingWith(string pattern)
        {
            return new Predicate<T>(obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameEndingWith(string pattern)
        {
            return new Predicate<T>(obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveNameContaining(string pattern)
        {
            return new Predicate<T>(obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> AreNotPrivate()
        {
            return new Predicate<T>(obj => obj.Visibility != Private, "are not private");
        }

        public static IPredicate<T> AreNotPublic()
        {
            return new Predicate<T>(obj => obj.Visibility != Public, "are not public");
        }

        public static IPredicate<T> AreNotProtected()
        {
            return new Predicate<T>(obj => obj.Visibility != Protected, "are not protected");
        }

        public static IPredicate<T> AreNotInternal()
        {
            return new Predicate<T>(obj => obj.Visibility != Internal, "are not internal");
        }

        public static IPredicate<T> AreNotProtectedInternal()
        {
            return new Predicate<T>(obj => obj.Visibility != ProtectedInternal, "are not protected internal");
        }

        public static IPredicate<T> AreNotPrivateProtected()
        {
            return new Predicate<T>(obj => obj.Visibility != PrivateProtected, "are not private protected");
        }
    }
}