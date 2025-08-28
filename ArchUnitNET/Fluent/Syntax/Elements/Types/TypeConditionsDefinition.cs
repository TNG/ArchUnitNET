using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Import;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Fluent.Syntax.DescriptionHelpers;
using Enum = ArchUnitNET.Domain.Enum;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypeConditionsDefinition<TRuleType>
        where TRuleType : IType
    {
        public static ICondition<TRuleType> Be(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return Be(types);
        }

        public static ICondition<TRuleType> Be(IEnumerable<Type> types)
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

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public static ICondition<TRuleType> BeAssignableTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            var description =
                "be assignable to types with full name "
                + (useRegularExpressions ? "matching " : "")
                + "\""
                + pattern
                + "\"";
            var failDescription =
                "is not assignable to a type with full name "
                + (useRegularExpressions ? "matching " : "")
                + "\""
                + pattern
                + "\"";
            return new SimpleCondition<TRuleType>(
                type => type.IsAssignableTo(pattern, useRegularExpressions),
                description,
                failDescription
            );
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use BeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public static ICondition<TRuleType> BeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.Any(pattern =>
                    ruleType.IsAssignableTo(pattern, useRegularExpressions)
                );
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
                failDescription = "is assignable to any type (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList
                    .Where(type => !type.Equals(firstPattern))
                    .Distinct()
                    .Aggregate(
                        "be assignable to types with full name "
                            + (useRegularExpressions ? "matching " : "")
                            + "\""
                            + firstPattern
                            + "\"",
                        (current, pattern) => current + " or \"" + pattern + "\""
                    );
                failDescription = patternList
                    .Where(type => !type.Equals(firstPattern))
                    .Distinct()
                    .Aggregate(
                        "is not assignable to types with full name "
                            + (useRegularExpressions ? "matching " : "")
                            + "\""
                            + firstPattern
                            + "\"",
                        (current, pattern) => current + " or \"" + pattern + "\""
                    );
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static RelationCondition<TRuleType, IType> BeTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                ObjectConditionsDefinition<TRuleType>.Be,
                "be types that",
                "are not types that"
            );
        }

        public static ICondition<TRuleType> BeAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static ICondition<TRuleType> BeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static ICondition<TRuleType> BeAssignableTo(IObjectProvider<IType> objectProvider)
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
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<IType> types)
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

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<Type> types)
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

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeNestedIn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeNestedIn(types);
        }

        public static ICondition<TRuleType> BeNestedIn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeNestedIn(types);
        }

        public static ICondition<TRuleType> BeNestedIn(IObjectProvider<IType> objectProvider)
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
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeNestedIn(IEnumerable<IType> types)
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

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeNestedIn(IEnumerable<Type> types)
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

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeValueTypes()
        {
            return new SimpleCondition<TRuleType>(
                type => type is Enum || type is Struct,
                "be value types",
                "is no value type"
            );
        }

        public static ICondition<TRuleType> BeEnums()
        {
            return new SimpleCondition<TRuleType>(type => type is Enum, "be enums", "is no enum");
        }

        public static ICondition<TRuleType> BeStructs()
        {
            return new SimpleCondition<TRuleType>(
                type => type is Struct,
                "be structs",
                "is no struct"
            );
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public static ICondition<TRuleType> ImplementInterface(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(pattern, useRegularExpressions),
                "implement interface with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\"",
                "does not implement interface with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> ImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"",
                "does not implement interface \"" + intf.FullName + "\""
            );
        }

        public static ICondition<TRuleType> ImplementInterface(Type intf)
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
            );
        }

        public static ICondition<TRuleType> ImplementAny(IObjectProvider<Interface> interfaces)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var interfaceList = interfaces.GetObjects(architecture).ToList();
                foreach (var ruleType in ruleTypes) {
                    if (interfaceList.Count > 0
                            ? ruleType.ImplementedInterfaces.Intersect(interfaceList).Any()
                            : ruleType.ImplementedInterfaces.Any())
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var failDescription =
                            !ruleType.ImplementedInterfaces.Any()
                                ? "does not implement any interface"
                                : "only implements "
                                  + string.Join(" and ", ruleType.ImplementedInterfaces.Select(i => i.FullName));
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = SelectDescription(
                "implement any interface",
                "implement",
                "implement any",
                interfaces
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        [Obsolete(
            "Either ResideInNamespace() without the useRegularExpressions parameter or ResideInNamespaceMatching() should be used"
        )]
        public static ICondition<TRuleType> ResideInNamespace(
            string pattern,
            bool useRegularExpressions
        )
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> ResideInNamespace(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(fullName),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static ICondition<TRuleType> ResideInNamespaceMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespaceMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        [Obsolete(
            "Either ResideInAssembly() without the useRegularExpressions parameter or ResideInAssemblyMatching() should be used"
        )]
        public static ICondition<TRuleType> ResideInAssembly(
            string pattern,
            bool useRegularExpressions
        )
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssembly(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> ResideInAssembly(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssembly(fullName),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static ICondition<TRuleType> ResideInAssemblyMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssemblyMatching(pattern),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> ResideInAssembly(
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

            return new ArchitectureCondition<TRuleType>(
                Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static ICondition<TRuleType> ResideInAssembly(
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

        public static ICondition<TRuleType> HavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasPropertyMemberWithName(name),
                "have a property member with name \"" + name + "\"",
                "does not have a property member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> AdhereToPlantUmlDiagram(Stream stream)
        {
            PlantUmlParsedDiagram diagram = new PlantUmlParser().Parse(stream);
            return createPlantUmlCondition(diagram);
        }

        public static ICondition<TRuleType> AdhereToPlantUmlDiagram(string file)
        {
            PlantUmlParsedDiagram diagram = new PlantUmlParser().Parse(file);
            return createPlantUmlCondition(diagram);
        }

        private static ICondition<TRuleType> createPlantUmlCondition(PlantUmlParsedDiagram diagram)
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

        public static ICondition<TRuleType> HaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasFieldMemberWithName(name),
                "have a field member with name \"" + name + "\"",
                "does not have a field member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> HaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMethodMemberWithName(name),
                "have a method member with name \"" + name + "\"",
                "does not have a method member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> HaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMemberWithName(name),
                "have a member with name \"" + name + "\"",
                "does not have a member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> BeNested()
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

        public static ICondition<TRuleType> NotBe(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBe(types);
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<Type> types)
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

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public static ICondition<TRuleType> NotBeAssignableTo(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var type in ruleType.GetAssignableTypes())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        dynamicFailDescription += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            var description =
                "not be assignable to types with full name "
                + (useRegularExpressions ? "matching " : "")
                + "\""
                + pattern
                + "\"";
            return new SimpleCondition<TRuleType>(Condition, description);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBeAssignableTo(Types().That().HaveFullName()) instead"
        )]
        public static ICondition<TRuleType> NotBeAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            var patternList = patterns.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var type in ruleType.GetAssignableTypes())
                {
                    if (
                        patternList.Any(pattern =>
                            type.FullNameMatches(pattern, useRegularExpressions)
                        )
                    )
                    {
                        dynamicFailDescription += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList
                    .Where(type => !type.Equals(firstPattern))
                    .Distinct()
                    .Aggregate(
                        "not be assignable to types with full name "
                            + (useRegularExpressions ? "matching " : "")
                            + "\""
                            + firstPattern
                            + "\"",
                        (current, pattern) => current + " or \"" + pattern + "\""
                    );
            }

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IObjectProvider<IType> objectProvider)
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
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<IType> types)
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

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<Type> types)
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

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeValueTypes()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Enum) && !(type is Struct),
                "not be value types",
                "is a value type"
            );
        }

        public static ICondition<TRuleType> NotBeEnums()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Enum),
                "not be enums",
                "is an enum"
            );
        }

        public static ICondition<TRuleType> NotBeStructs()
        {
            return new SimpleCondition<TRuleType>(
                type => !(type is Struct),
                "not be structs",
                "is a struct"
            );
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public static ICondition<TRuleType> NotImplementInterface(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ImplementsInterface(pattern, useRegularExpressions),
                "not implement interface with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\"",
                "does implement interface with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> NotImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ImplementsInterface(intf),
                "not implement interface \"" + intf.FullName + "\"",
                "does implement interface \"" + intf.FullName + "\""
            );
        }

        public static ICondition<TRuleType> NotImplementInterface(Type intf)
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
            );
        }

        public static ICondition<TRuleType> NotImplementAny(IObjectProvider<Interface> interfaces)
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

            var description = SelectDescription(
                "not implement any interface",
                "not implement",
                "not implement any",
                interfaces
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        [Obsolete(
            "Either NotResideInNamespace() without the useRegularExpressions parameter or NotResideInNamespaceMatching() should be used"
        )]
        public static ICondition<TRuleType> NotResideInNamespace(
            string pattern,
            bool useRegularExpressions
        )
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> NotResideInNamespace(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(fullName),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name \"" + fullName + "\""
            );
        }

        public static ICondition<TRuleType> NotResideInNamespaceMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespaceMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name matching \"" + pattern + "\""
            );
        }

        [Obsolete(
            "Either NotResideInAssembly() without the useRegularExpressions parameter or NotResideInAssemblyMatching() should be used"
        )]
        public static ICondition<TRuleType> NotResideInAssembly(
            string pattern,
            bool useRegularExpressions
        )
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssembly(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> NotResideInAssembly(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssembly(fullName),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name \"" + fullName + "\""
            );
        }

        public static ICondition<TRuleType> NotResideInAssemblyMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssemblyMatching(pattern),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotResideInAssembly(
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

            return new ArchitectureCondition<TRuleType>(
                Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName,
                description
            );
        }

        public static ICondition<TRuleType> NotResideInAssembly(
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

        public static ICondition<TRuleType> NotHavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasPropertyMemberWithName(name),
                "not have property member with name \"" + name + "\"",
                "does have property member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasFieldMemberWithName(name),
                "not have field member with name \"" + name + "\"",
                "does have field member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMethodMemberWithName(name),
                "not have method member with name \"" + name + "\"",
                "does have method member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMemberWithName(name),
                "not have member with name \"" + name + "\"",
                "does have member with name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> NotBeNested()
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
