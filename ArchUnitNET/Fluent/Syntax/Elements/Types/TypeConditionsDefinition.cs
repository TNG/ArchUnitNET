using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Import;
using ArchUnitNET.Fluent.Conditions;
using Enum = ArchUnitNET.Domain.Enum;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypeConditionsDefinition<TRuleType>
        where TRuleType : IType
    {
        public static IOrderedCondition<TRuleType> Be(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return Be(types);
        }

        public static IOrderedCondition<TRuleType> Be(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
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

                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .OfType<IType>()
                    .Intersect(archUnitTypeList)
                    .ToList();
                foreach (var failedObject in ruleTypeList.Cast<IType>().Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "is " + failedObject.FullName
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "be \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static RelationCondition<TRuleType, IType> BeTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                ObjectConditionsDefinition<TRuleType>.Be,
                "be types that",
                "are not types that"
            );
        }

        public static IOrderedCondition<TRuleType> BeAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static IOrderedCondition<TRuleType> BeAssignableTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static IOrderedCondition<TRuleType> BeAssignableTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is not assignable to " + objectProvider.Description;
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is assignable to any type (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !type.Equals(firstType))
                        .Distinct()
                        .Aggregate(
                            "is not assignable to \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "be assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<TRuleType>(Condition, description).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
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

                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(archUnitTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is assignable to any type (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "is not assignable to \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "be assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeNestedIn(types);
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeNestedIn(types);
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type =>
                        typeList.Any(outerType =>
                            type.FullName.StartsWith(outerType.FullName + "+")
                        )
                    )
                    .ToList();
                var failDescription = "is not nested in " + objectProvider.Description;
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be nested in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type =>
                        typeList.Any(outerType =>
                            type.FullName.StartsWith(outerType.FullName + "+")
                        )
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is nested in any type (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !type.Equals(firstType))
                        .Distinct()
                        .Aggregate(
                            "is not nested in \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be nested in no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "be nested in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<TRuleType>(Condition, description).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type =>
                        typeList.Any(outerType =>
                            type.FullName.StartsWith(outerType.FullName + "+")
                        )
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is nested in any type (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "is not nested in \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be nested in no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "be nested in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> BeValueTypes()
        {
            return new SimpleCondition<TRuleType>(
                type => type is Enum || type is Struct,
                "be value types",
                "is no value type"
            );
        }

        public static IOrderedCondition<TRuleType> BeEnums()
        {
            return new SimpleCondition<TRuleType>(type => type is Enum, "be enums", "is no enum");
        }

        public static IOrderedCondition<TRuleType> BeStructs()
        {
            return new SimpleCondition<TRuleType>(
                type => type is Struct,
                "be structs",
                "is no struct"
            );
        }

        public static IOrderedCondition<TRuleType> ImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"",
                "does not implement interface \"" + intf.FullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> ImplementInterface(Type intf)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var ruleTypeList = ruleTypes.ToList();
                Interface archUnitInterface = null;
                var interfaceNotInArchitecture = false;
                try
                {
                    archUnitInterface = architecture.GetInterfaceOfType(intf);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    interfaceNotInArchitecture = true;
                }

                if (interfaceNotInArchitecture)
                {
                    foreach (var ruleType in ruleTypeList)
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            "does not implement interface \"" + intf.FullName + "\""
                        );
                    }

                    yield break;
                }

                var passedObjects = ruleTypeList
                    .Where(type => type.ImplementsInterface(archUnitInterface))
                    .ToList();

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "does not implement interface \"" + intf.FullName + "\""
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                "implement interface \"" + intf.FullName + "\""
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> ImplementAny(
            IObjectProvider<Interface> interfaces
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var interfaceList = interfaces.GetObjects(architecture).ToList();
                foreach (var ruleType in ruleTypes)
                {
                    if (
                        interfaceList.Count > 0
                            ? ruleType.ImplementedInterfaces.Intersect(interfaceList).Any()
                            : ruleType.ImplementedInterfaces.Any()
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var failDescription = !ruleType.ImplementedInterfaces.Any()
                            ? "does not implement any interface"
                            : "only implements "
                                + string.Join(
                                    " and ",
                                    ruleType.ImplementedInterfaces.Select(i => i.FullName)
                                );
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = interfaces.FormatDescription(
                "implement any interface",
                "implement",
                "implement any"
            );
            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> ResideInNamespace(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(fullName),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> ResideInNamespaceMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespaceMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        public static IOrderedCondition<TRuleType> ResideInAssembly(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssembly(fullName),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> ResideInAssemblyMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssemblyMatching(pattern),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static IOrderedCondition<TRuleType> ResideInAssembly(
            System.Reflection.Assembly assembly,
            params System.Reflection.Assembly[] moreAssemblies
        )
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly))
                    || moreAssemblies.Any(asm =>
                        ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm))
                    );
            }

            var description = moreAssemblies.Aggregate(
                "reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new OrderedArchitectureCondition<TRuleType>(
                Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static IOrderedCondition<TRuleType> ResideInAssembly(
            Assembly assembly,
            Assembly[] moreAssemblies
        )
        {
            bool Condition(TRuleType ruleType)
            {
                var assemblies = new[] { assembly }.Concat(moreAssemblies);
                return assemblies.Contains(ruleType.Assembly);
            }

            var description = moreAssemblies.Aggregate(
                "reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new SimpleCondition<TRuleType>(
                Condition,
                type => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static IOrderedCondition<TRuleType> HavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasPropertyMemberWithName(name),
                "have a property member with name \"" + name + "\"",
                "does not have a property member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> AdhereToPlantUmlDiagram(Stream stream)
        {
            PlantUmlParsedDiagram diagram = new PlantUmlParser().Parse(stream);
            return createPlantUmlCondition(diagram);
        }

        public static IOrderedCondition<TRuleType> AdhereToPlantUmlDiagram(string file)
        {
            PlantUmlParsedDiagram diagram = new PlantUmlParser().Parse(file);
            return createPlantUmlCondition(diagram);
        }

        private static IOrderedCondition<TRuleType> createPlantUmlCondition(
            PlantUmlParsedDiagram diagram
        )
        {
            ClassDiagramAssociation classDiagramAssociation = new ClassDiagramAssociation(diagram);

            ConditionResult Condition(TRuleType ruleType)
            {
                if (ruleType.Dependencies.All(d => !classDiagramAssociation.Contains(d.Target)))
                {
                    return new ConditionResult(ruleType, true);
                }

                List<string> allAllowedTargets = new List<string>();

                allAllowedTargets.AddRange(
                    classDiagramAssociation
                        .GetNamespaceIdentifiersFromComponentOf(ruleType)
                        .Concat(classDiagramAssociation.GetTargetNamespaceIdentifiers(ruleType))
                        .ToList()
                );

                var pass = true;
                var dynamicFailDescription = "does depend on";

                //Prevent failDescriptions like "does depend on X and does depend on X and does depend on Y and does depend on Y
                var ruleTypeDependencies = ruleType
                    .GetTypeDependencies()
                    .GroupBy(p => p.FullName)
                    .Select(g => g.First());
                foreach (var dependency in ruleTypeDependencies)
                {
                    if (
                        classDiagramAssociation.Contains(dependency)
                        && !allAllowedTargets.Any(pattern => dependency.FullNameMatches(pattern))
                    )
                    {
                        dynamicFailDescription += pass
                            ? " " + dependency.FullName
                            : " and " + dependency.FullName;

                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            return new SimpleCondition<TRuleType>(Condition, "adhere to PlantUML diagram.");
        }

        public static IOrderedCondition<TRuleType> HaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasFieldMemberWithName(name),
                "have a field member with name \"" + name + "\"",
                "does not have a field member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> HaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMethodMemberWithName(name),
                "have a method member with name \"" + name + "\"",
                "does not have a method member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> HaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMemberWithName(name),
                "have a member with name \"" + name + "\"",
                "does not have a member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> BeNested()
        {
            return new SimpleCondition<TRuleType>(
                type => type.IsNested,
                "be nested",
                "is not nested"
            );
        }

        //Relation Conditions

        public static RelationCondition<TRuleType, IType> BeAssignableToTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                BeAssignableTo,
                "be assignable to types that",
                "is not assignable to types that"
            );
        }

        public static RelationCondition<TRuleType, Interface> ImplementAnyInterfacesThat()
        {
            return new RelationCondition<TRuleType, Interface>(
                ImplementAny,
                "implement any interfaces that",
                "does not implement any interfaces that"
            );
        }

        //Negations

        public static IOrderedCondition<TRuleType> NotBe(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBe(types);
        }

        public static IOrderedCondition<TRuleType> NotBe(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
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

                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .OfType<IType>()
                    .Intersect(archUnitTypeList)
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "is " + failedObject.FullName
                    );
                }

                foreach (var passedObject in ruleTypeList.Cast<IType>().Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "not be \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotBeAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static IOrderedCondition<TRuleType> NotBeAssignableTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static IOrderedCondition<TRuleType> NotBeAssignableTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is assignable to " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not be assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotBeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                string dynamicFailDescription;
                if (typeList.IsNullOrEmpty())
                {
                    dynamicFailDescription = "is assignable to any type (always true)";
                    foreach (var failedObject in failedObjects)
                    {
                        yield return new ConditionResult(
                            failedObject,
                            false,
                            dynamicFailDescription
                        );
                    }
                }
                else
                {
                    foreach (var failedObject in failedObjects)
                    {
                        dynamicFailDescription = "is assignable to";
                        var first = true;
                        foreach (var type in failedObject.GetAssignableTypes().Intersect(typeList))
                        {
                            dynamicFailDescription += first
                                ? " " + type.FullName
                                : " and " + type.FullName;
                            first = false;
                        }

                        yield return new ConditionResult(
                            failedObject,
                            false,
                            dynamicFailDescription
                        );
                    }
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "not be assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<TRuleType>(Condition, description).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotBeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
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

                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetAssignableTypes().Intersect(archUnitTypeList).Any())
                    .ToList();
                string dynamicFailDescription;
                if (typeList.IsNullOrEmpty())
                {
                    dynamicFailDescription = "is assignable to any type (always true)";
                    foreach (var failedObject in failedObjects)
                    {
                        yield return new ConditionResult(
                            failedObject,
                            false,
                            dynamicFailDescription
                        );
                    }
                }
                else
                {
                    foreach (var failedObject in failedObjects)
                    {
                        dynamicFailDescription = "is assignable to";
                        var first = true;
                        foreach (
                            var type in failedObject
                                .GetAssignableTypes()
                                .Intersect(archUnitTypeList)
                        )
                        {
                            dynamicFailDescription += first
                                ? " " + type.FullName
                                : " and " + type.FullName;
                            first = false;
                        }

                        yield return new ConditionResult(
                            failedObject,
                            false,
                            dynamicFailDescription
                        );
                    }
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "not be assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotBeValueTypes()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Enum) && !(type is Struct),
                "not be value types",
                "is a value type"
            );
        }

        public static IOrderedCondition<TRuleType> NotBeEnums()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Enum),
                "not be enums",
                "is an enum"
            );
        }

        public static IOrderedCondition<TRuleType> NotBeStructs()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Struct),
                "not be structs",
                "is a struct"
            );
        }

        public static IOrderedCondition<TRuleType> NotImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ImplementsInterface(intf),
                "not implement interface \"" + intf.FullName + "\"",
                "does implement interface \"" + intf.FullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotImplementInterface(Type intf)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var ruleTypeList = ruleTypes.ToList();
                Interface archUnitInterface = null;
                var interfaceNotInArchitecture = false;
                try
                {
                    archUnitInterface = architecture.GetInterfaceOfType(intf);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    interfaceNotInArchitecture = true;
                }

                if (interfaceNotInArchitecture)
                {
                    foreach (var ruleType in ruleTypeList)
                    {
                        yield return new ConditionResult(ruleType, true);
                    }

                    yield break;
                }

                var passedObjects = ruleTypeList
                    .Where(type => !type.ImplementsInterface(archUnitInterface))
                    .ToList();

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "does implement interface \"" + intf.FullName + "\""
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new ArchitectureCondition<TRuleType>(
                Condition,
                "not implement interface \"" + intf.FullName + "\""
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotImplementAny(
            IObjectProvider<Interface> interfaces
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var interfaceList = interfaces.GetObjects(architecture).ToList();
                foreach (var ruleType in ruleTypes.ToList())
                {
                    var matchingInterfaces =
                        interfaceList.Count > 0
                            ? ruleType.ImplementedInterfaces.Intersect(interfaceList).ToList()
                            : ruleType.ImplementedInterfaces.ToList();
                    if (matchingInterfaces.Any())
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            "does implement "
                                + string.Join(" and ", matchingInterfaces.Select(i => i.FullName))
                        );
                    }
                    else
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                }
            }

            var description = interfaces.FormatDescription(
                "not implement any interface",
                "not implement",
                "not implement any"
            );
            return new ArchitectureCondition<TRuleType>(
                Condition,
                description
            ).AsOrderedCondition();
        }

        public static IOrderedCondition<TRuleType> NotResideInNamespace(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(fullName),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotResideInNamespaceMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespaceMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotResideInAssembly(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssembly(fullName),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotResideInAssemblyMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssemblyMatching(pattern),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotResideInAssembly(
            System.Reflection.Assembly assembly,
            params System.Reflection.Assembly[] moreAssemblies
        )
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly))
                    && !moreAssemblies.Any(asm =>
                        ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm))
                    );
            }

            var description = moreAssemblies.Aggregate(
                "not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new OrderedArchitectureCondition<TRuleType>(
                Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static IOrderedCondition<TRuleType> NotResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            bool Condition(TRuleType ruleType)
            {
                var assemblies = new[] { assembly }.Concat(moreAssemblies);
                return !assemblies.Contains(ruleType.Assembly);
            }

            var description = moreAssemblies.Aggregate(
                "not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\""
            );

            return new SimpleCondition<TRuleType>(
                Condition,
                type => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static IOrderedCondition<TRuleType> NotHavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasPropertyMemberWithName(name),
                "not have property member with name \"" + name + "\"",
                "does have property member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotHaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasFieldMemberWithName(name),
                "not have field member with name \"" + name + "\"",
                "does have field member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotHaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMethodMemberWithName(name),
                "not have method member with name \"" + name + "\"",
                "does have method member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotHaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMemberWithName(name),
                "not have member with name \"" + name + "\"",
                "does have member with name \"" + name + "\""
            );
        }

        public static IOrderedCondition<TRuleType> NotBeNested()
        {
            return new SimpleCondition<TRuleType>(
                type => !type.IsNested,
                "not be nested",
                "is nested"
            );
        }

        //Relation Condition Negations

        public static RelationCondition<TRuleType, IType> NotBeAssignableToTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                NotBeAssignableTo,
                "not be assignable to types that",
                "is assignable to types that"
            );
        }

        public static RelationCondition<TRuleType, Interface> NotImplementAnyInterfacesThat() =>
            new RelationCondition<TRuleType, Interface>(
                NotImplementAny,
                "not implement any interfaces that",
                "does implement any interfaces that"
            );
    }
}
