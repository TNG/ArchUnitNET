﻿using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberPredicatesDefinition<T>
        where T : IMember
    {
        public static IPredicate<T> AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return AreDeclaredIn(types);
        }

        public static IPredicate<T> AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return AreDeclaredIn(types);
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
                description = typeList
                    .Where(obj => !obj.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }
                return members.Intersect(
                    archUnitTypeList.SelectMany(type => type.Members).OfType<T>()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "are declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreStatic()
        {
            return new SimplePredicate<T>(
                member => member.IsStatic.HasValue && member.IsStatic.Value,
                "are static"
            );
        }

        public static IPredicate<T> AreReadOnly()
        {
            return new SimplePredicate<T>(
                member => member.Writability == Writability.ReadOnly,
                "are read only"
            );
        }

        public static IPredicate<T> AreImmutable()
        {
            return new SimplePredicate<T>(
                member => member.Writability.IsImmutable(),
                "are immutable"
            );
        }

        //Negations

        public static IPredicate<T> AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return AreNotDeclaredIn(types);
        }

        public static IPredicate<T> AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return AreNotDeclaredIn(types);
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
                description = typeList
                    .Where(obj => !obj.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are not declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> members, Architecture architecture)
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }
                return members.Except(
                    archUnitTypeList.SelectMany(type => type.Members).OfType<T>()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in any type (always true9";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "are not declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotStatic()
        {
            return new SimplePredicate<T>(
                member => member.IsStatic.HasValue && !member.IsStatic.Value,
                "are not static"
            );
        }

        public static IPredicate<T> AreNotReadOnly()
        {
            return new SimplePredicate<T>(
                member => member.Writability != Writability.ReadOnly,
                "are not read only"
            );
        }

        public static IPredicate<T> AreNotImmutable()
        {
            return new SimplePredicate<T>(
                member => !member.Writability.IsImmutable(),
                "are not immutablee"
            );
        }
    }
}
