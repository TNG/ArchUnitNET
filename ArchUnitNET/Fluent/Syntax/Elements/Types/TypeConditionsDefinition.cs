using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Import;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Extensions.EnumerableExtensions;
using Enum = ArchUnitNET.Domain.Enum;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    internal static class TypeConditionsDefinition<TRuleType>
        where TRuleType : IType
    {
        public static IOrderedCondition<TRuleType> Be(IObjectProvider<IType> objectProvider)
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var isAllowedType = CreateLookupFn(typeList);
                foreach (var ruleType in ruleTypes)
                {
                    if (isAllowedType(ruleType))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "does exist"
                                : "is not " + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription("not exist", "be", "be");
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var isExpectedType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                var failDescription = "is not assignable to " + objectProvider.Description;
                foreach (var ruleType in ruleTypes)
                {
                    if (ruleType.GetAssignableTypes().Any(isExpectedType))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "be assignable to no types (always false)",
                "be assignable to",
                "be assignable to"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
        }

        public static IOrderedCondition<TRuleType> BeNestedIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var failDescription = "is not nested in " + objectProvider.Description;
                foreach (var ruleType in ruleTypes)
                {
                    if (
                        typeList.Any(outerType =>
                            ruleType.FullName.StartsWith(outerType.FullName + "+")
                        )
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "be nested in no types (always false)",
                "be nested in",
                "be nested in"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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

                foreach (var ruleType in ruleTypes)
                {
                    if (interfaceNotInArchitecture)
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            "does not implement interface \"" + intf.FullName + "\""
                        );
                    }
                    else if (ruleType.ImplementsInterface(archUnitInterface))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            "does not implement interface \"" + intf.FullName + "\""
                        );
                    }
                }
            }

            return new OrderedArchitectureCondition<TRuleType>(
                Condition,
                "implement interface \"" + intf.FullName + "\""
            );
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
                        && ruleType.ImplementedInterfaces.Intersect(interfaceList).Any()
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
                "implement any of no interfaces (always false)",
                "implement",
                "implement any"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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

        public static IOrderedCondition<TRuleType> NotBe(IObjectProvider<IType> objectProvider)
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var isForbiddenType = CreateLookupFn(typeList);
                foreach (var ruleType in ruleTypes)
                {
                    if (!isForbiddenType(ruleType))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "does exist"
                                : "is " + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription("exist", "not be", "not be");
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                var failDescription = "is assignable to " + objectProvider.Description;
                foreach (var ruleType in ruleTypes)
                {
                    if (ruleType.GetAssignableTypes().Any(isForbiddenType))
                    {
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                    else
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "not be assignable to no types (always true)",
                "not be assignable to",
                "not be assignable to"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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

                foreach (var ruleType in ruleTypes)
                {
                    if (interfaceNotInArchitecture)
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else if (!ruleType.ImplementsInterface(archUnitInterface))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            ruleType,
                            false,
                            "does implement interface \"" + intf.FullName + "\""
                        );
                    }
                }
            }

            return new OrderedArchitectureCondition<TRuleType>(
                Condition,
                "not implement interface \"" + intf.FullName + "\""
            );
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
                foreach (var ruleType in ruleTypes)
                {
                    var matchingInterfaces = ruleType.ImplementedInterfaces.Intersect(
                        interfaceList
                    );
                    if (interfaceList.Count > 0 && matchingInterfaces.Any())
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
                "not implement any of no interfaces (always true)",
                "not implement",
                "not implement any"
            );
            return new OrderedArchitectureCondition<TRuleType>(Condition, description);
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
