using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using Assembly = System.Reflection.Assembly;
using Enum = ArchUnitNET.Domain.Enum;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypePredicatesDefinition<T>
        where T : IType
    {
        public static IPredicate<T> Are(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return Are(types);
        }

        public static IPredicate<T> Are(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> ruleTypes, Architecture architecture)
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
                        //ignore, can't be equal anyways
                    }
                }

                return ruleTypes.Intersect(archUnitTypeList.OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "are \"" + firstType.FullName + "\"",
                        (current, obj) => current + " or \"" + obj.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                var types = moreTypes.Concat(new[] { firstType });
                return ruleTypes.Where(type => type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = moreTypes.Aggregate(
                "are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = moreTypes
                    .Concat(new[] { firstType })
                    .Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type => type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = moreTypes.Aggregate(
                "are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = "are assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                return ruleTypes.Where(type => type.GetAssignableTypes().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
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

                return ruleTypes.Where(type =>
                    type.GetAssignableTypes().Intersect(archUnitTypeList).Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "are assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNestedIn(IType firstType, params IType[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                var types = moreTypes.Concat(new[] { firstType });
                return ruleTypes.Where(type =>
                    types.Any(outerType => type.FullName.StartsWith(outerType.FullName + "+"))
                );
            }

            var description = moreTypes.Aggregate(
                "are nested in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNestedIn(Type firstType, params Type[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = moreTypes
                    .Concat(new[] { firstType })
                    .Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type =>
                    types.Any(outerType => type.FullName.StartsWith(outerType.FullName + "+"))
                );
            }
            var description = moreTypes.Aggregate(
                "are nested in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNestedIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type =>
                    types.Any(outerType => type.FullName.StartsWith(outerType.FullName + "+"))
                );
            }

            var description = "are nested in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNestedIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    typeList.Any(outerType => type.FullName.StartsWith(outerType.FullName + "+"))
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are nested in no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are nested in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNestedIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                return ruleTypes.Where(type =>
                    typeList.Any(outerType => type.FullName.StartsWith(outerType.FullName + "+"))
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are nested in no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "are nested in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreValueTypes()
        {
            return new SimplePredicate<T>(
                type => type is Enum || type is Struct,
                "are value types"
            );
        }

        public static IPredicate<T> AreEnums()
        {
            return new SimplePredicate<T>(type => type is Enum, "are enums");
        }

        public static IPredicate<T> AreStructs()
        {
            return new SimplePredicate<T>(type => type is Struct, "are structs");
        }

        public static IPredicate<T> ImplementInterface(Interface intf)
        {
            return new SimplePredicate<T>(
                type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\""
            );
        }

        public static IPredicate<T> ImplementInterface(Type intf)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                Interface archUnitInterface;
                try
                {
                    archUnitInterface = architecture.GetInterfaceOfType(intf);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return Enumerable.Empty<T>();
                }

                return ruleTypes.Where(type => type.ImplementsInterface(archUnitInterface));
            }

            return new ArchitecturePredicate<T>(
                Condition,
                "implement interface \"" + intf.FullName + "\""
            );
        }

        public static IPredicate<T> ImplementAny(IObjectProvider<Interface> interfaces)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var interfaceList = interfaces.GetObjects(architecture).ToList();
                return ruleTypes.Where(type =>
                    interfaceList.Count > 0
                        ? type.ImplementedInterfaces.Intersect(interfaceList).Any()
                        : type.ImplementedInterfaces.Any()
                );
            }
            var description = interfaces.FormatDescription(
                "implement any interface",
                "implement",
                "implement any"
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> ResideInNamespace(string fullName)
        {
            return new SimplePredicate<T>(
                type => type.ResidesInNamespace(fullName),
                "reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> ResideInNamespaceMatching(string pattern)
        {
            return new SimplePredicate<T>(
                type => type.ResidesInNamespaceMatching(pattern),
                "reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> ResideInAssembly(string fullName)
        {
            return new SimplePredicate<T>(
                type => type.ResidesInAssembly(fullName),
                "reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> ResideInAssemblyMatching(string pattern)
        {
            return new SimplePredicate<T>(
                type => type.ResidesInAssemblyMatching(pattern),
                "reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> ResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            IEnumerable<T> Condition(IEnumerable<T> types, Architecture architecture)
            {
                var assemblyList = moreAssemblies
                    .Concat(new[] { assembly })
                    .Select(architecture.GetAssemblyOfAssembly);
                return types.Where(type => assemblyList.Contains(type.Assembly));
            }

            var description = moreAssemblies.Aggregate(
                "reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            IEnumerable<T> Condition(IEnumerable<T> types)
            {
                var assemblyList = moreAssemblies.Concat(new[] { assembly });
                return types.Where(type => assemblyList.Contains(type.Assembly));
            }

            var description = moreAssemblies.Aggregate(
                "reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> HavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\""
            );
        }

        public static IPredicate<T> HaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> HaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> HaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> AreNested()
        {
            return new SimplePredicate<T>(type => type.IsNested, "are nested");
        }

        //Negations

        public static IPredicate<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            return AreNot(new List<Type>() { firstType }.Concat(moreTypes));
        }

        public static IPredicate<T> AreNot(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Filter(IEnumerable<T> ruleTypes, Architecture architecture)
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
                        //ignore, can't be equal anyways
                    }
                }

                return ruleTypes.Except(archUnitTypeList.OfType<T>());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "are not \"" + firstType.FullName + "\"",
                        (current, obj) => current + " or \"" + obj.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                var types = moreTypes.Concat(new[] { firstType });
                return ruleTypes.Where(type => !type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = moreTypes.Aggregate(
                "are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = moreTypes
                    .Concat(new[] { firstType })
                    .Select(architecture.GetITypeOfType);
                return ruleTypes.Where(type => !type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = moreTypes.Aggregate(
                "are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => !type.GetAssignableTypes().Intersect(types).Any());
            }

            var description = "are not assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    !type.GetAssignableTypes().Intersect(typeList).Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are not assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
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

                return ruleTypes.Where(type =>
                    !type.GetAssignableTypes().Intersect(archUnitTypeList).Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "are not assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotValueTypes()
        {
            return new SimplePredicate<T>(
                cls => !(cls is Enum) && !(cls is Struct),
                "are not value types"
            );
        }

        public static IPredicate<T> AreNotEnums()
        {
            return new SimplePredicate<T>(cls => !(cls is Enum), "are not enums");
        }

        public static IPredicate<T> AreNotStructs()
        {
            return new SimplePredicate<T>(cls => !(cls is Struct), "are not structs");
        }

        public static IPredicate<T> DoNotImplementInterface(Interface intf)
        {
            return new SimplePredicate<T>(
                type => !type.ImplementsInterface(intf),
                "do not implement interface \"" + intf.FullName + "\""
            );
        }

        public static IPredicate<T> DoNotImplementInterface(Type intf)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                Interface archUnitInterface;
                try
                {
                    archUnitInterface = architecture.GetInterfaceOfType(intf);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return ruleTypes;
                }

                return ruleTypes.Where(type => !type.ImplementsInterface(archUnitInterface));
            }

            return new ArchitecturePredicate<T>(
                Condition,
                "do not implement interface \"" + intf.FullName + "\""
            );
        }

        public static IPredicate<T> DoNotImplementAny(IObjectProvider<Interface> interfaces)
        {
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var interfaceList = interfaces.GetObjects(architecture).ToList();
                return ruleTypes.Where(type =>
                    interfaceList.Count > 0
                        ? !type.ImplementedInterfaces.Intersect(interfaceList).Any()
                        : !type.ImplementedInterfaces.Any()
                );
            }
            var description = interfaces.FormatDescription(
                "do not implement any interface",
                "do not implement",
                "do not implement any"
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotResideInNamespace(string fullName)
        {
            return new SimplePredicate<T>(
                type => !type.ResidesInNamespace(fullName),
                "do not reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> DoNotResideInNamespaceMatching(string pattern)
        {
            return new SimplePredicate<T>(
                type => !type.ResidesInNamespaceMatching(pattern),
                "do not reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotResideInAssembly(string fullName)
        {
            return new SimplePredicate<T>(
                type => !type.ResidesInAssembly(fullName),
                "do not reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> DoNotResideInAssemblyMatching(string pattern)
        {
            return new SimplePredicate<T>(
                type => !type.ResidesInAssemblyMatching(pattern),
                "do not reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            IEnumerable<T> Condition(IEnumerable<T> types, Architecture architecture)
            {
                var assemblyList = moreAssemblies
                    .Concat(new[] { assembly })
                    .Select(architecture.GetAssemblyOfAssembly);
                return types.Where(type => !assemblyList.Contains(type.Assembly));
            }

            var description = moreAssemblies.Aggregate(
                "do not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            IEnumerable<T> Condition(IEnumerable<T> types)
            {
                var assemblyList = moreAssemblies.Concat(new[] { assembly });
                return types.Where(type => !assemblyList.Contains(type.Assembly));
            }

            var description = moreAssemblies.Aggregate(
                "do not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new EnumerablePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> DoNotHaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(
                type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\""
            );
        }

        public static IPredicate<T> AreNotNested()
        {
            return new SimplePredicate<T>(type => !type.IsNested, "are not nested");
        }
    }
}
