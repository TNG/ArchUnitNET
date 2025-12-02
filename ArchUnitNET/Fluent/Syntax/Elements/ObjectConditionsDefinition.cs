using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    internal static class ObjectConditionsDefinition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public static ICondition<TRuleType> Exist()
        {
            return new ExistsCondition<TRuleType>(true);
        }

        public static ICondition<TRuleType> Be(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<ICanBeAnalyzed>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var objectList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var passedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                            ? "does exist"
                            : "is not " + objectProvider.Description
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription("not exist", "be", "be");
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> CallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var methodList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var passedObjects = typeList
                    .Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    var calledMethods = failedObject.GetCalledMethods().ToList();
                    var dynamicFailDescription =
                        calledMethods.Count == 0
                            ? "does not call any methods"
                            : "only calls "
                                + string.Join(
                                    " and ",
                                    calledMethods.Select(method => $"\"{method.FullName}\"")
                                );
                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }
                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }
            var description = objectProvider.FormatDescription(
                "call any of no methods (impossible)",
                "call",
                "call any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var dependants = failedObject.GetTypeDependencies(architecture).ToList();
                    var dynamicFailDescription =
                        dependants.Count == 0
                            ? "does not depend on any type"
                            : "only depends on "
                                + string.Join(
                                    " and ",
                                    dependants.Select(type => $"\"{type.FullName}\"")
                                );
                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }
                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }
            var description = objectProvider.FormatDescription(
                "depend on any of no types (impossible)",
                "depend on",
                "depend on any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> FollowCustomCondition(
            Func<TRuleType, ConditionResult> condition,
            string description
        )
        {
            return new SimpleCondition<TRuleType>(condition, description);
        }

        public static ICondition<TRuleType> FollowCustomCondition(
            Func<TRuleType, bool> condition,
            string description,
            string failDescription
        )
        {
            return new SimpleCondition<TRuleType>(condition, description, failDescription);
        }

        public static ICondition<TRuleType> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies(architecture).Except(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(typeList))
                    {
                        dynamicFailDescription += first
                            ? " " + type.FullName
                            : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "have no dependencies",
                "only depend on",
                "only depend on"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var attributes = failedObject.Attributes.ToList();
                    var failDescription =
                        attributes.Count == 0
                            ? "does not have any attributes"
                            : "only has attributes "
                                + string.Join(
                                    " and ",
                                    attributes.Select(attribute => $"\"{attribute.FullName}\"")
                                );
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "have any of no attributes (impossible)",
                "have",
                "have any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.Attributes.Except(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Except(attributeList))
                    {
                        dynamicFailDescription += first
                            ? " " + attribute.FullName
                            : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "have no attributes",
                "only have",
                "only have any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    if (
                        ruleType.AttributeInstances.Any(instance =>
                        {
                            var instanceArgumentValues = instance
                                .GetAllAttributeArgumentValues()
                                .ToList();
                            return resolvedArgumentValueList.All(instanceArgumentValues.Contains);
                        })
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var argumentsDescriptions = ruleType.AttributeInstances.Select(instance =>
                        {
                            var argumentsDescription = instance
                                .GetAllAttributeArgumentValues()
                                .FormatDescription(
                                    "without arguments",
                                    "with argument",
                                    "with arguments"
                                );
                            return $"{instance.Type.FullName} {argumentsDescription}";
                        });
                        var failDescription = argumentsDescriptions.FormatDescription(
                            "does not have any attributes",
                            "does only have attribute",
                            "does only have attributes",
                            elementDescription: str => str
                        );
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }
            var description = argumentValueList.FormatDescription(
                "have any attributes",
                "have any attributes with argument",
                "have any attributes with arguments"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            [NotNull] IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var matchingAttributeInstances = ruleType
                        .AttributeInstances.Where(instance =>
                            instance.Type.Equals(resolvedAttribute)
                        )
                        .ToList();
                    if (
                        matchingAttributeInstances.Any(instance =>
                        {
                            var instanceArgumentValues = instance
                                .GetAllAttributeArgumentValues()
                                .ToList();
                            return resolvedArgumentValueList.All(instanceArgumentValues.Contains);
                        })
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var argumentsDescriptions = matchingAttributeInstances.Select(instance =>
                            instance
                                .GetAllAttributeArgumentValues()
                                .FormatDescription(
                                    "without arguments",
                                    "with argument",
                                    "with arguments"
                                )
                        );
                        var failDescription = argumentsDescriptions.FormatDescription(
                            $"does not have attribute \"{resolvedAttribute.FullName}\"",
                            $"does only have attribute \"{resolvedAttribute.FullName}\"",
                            $"does only have attribute \"{resolvedAttribute.FullName}\"",
                            elementDescription: str => str
                        );
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }
            var description = argumentValueList.FormatDescription(
                $"have attribute \"{attributeFullName}\"",
                $"have attribute \"{attributeFullName}\" with argument",
                $"have attribute \"{attributeFullName}\" with arguments"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    if (
                        ruleType.AttributeInstances.Any(instance =>
                        {
                            var attArguments = instance
                                .GetAllNamedAttributeArgumentTuples()
                                .ToList();
                            return resolvedNamedArgumentList.All(attArguments.Contains);
                        })
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var argumentsDescriptions = ruleType.AttributeInstances.Select(instance =>
                        {
                            var argumentsDescription = ruleType
                                .GetAllNamedAttributeArgumentTuples(instance.Type)
                                .FormatDescription(
                                    "without named arguments",
                                    "with named argument",
                                    "with named arguments",
                                    elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
                                );
                            return $"{instance.Type.FullName} {argumentsDescription}";
                        });
                        var failDescription = argumentsDescriptions.FormatDescription(
                            "does not have any attribute",
                            "does only have attribute",
                            "does only have attributes",
                            elementDescription: str => str
                        );
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }
            var description = namedArgumentList.FormatDescription(
                "have any attributes",
                "have any attributes with named argument",
                "have any attributes with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var matchingAttributeInstances = ruleType
                        .AttributeInstances.Where(instance =>
                            instance.Type.Equals(resolvedAttribute)
                        )
                        .ToList();
                    if (
                        matchingAttributeInstances.Any(instance =>
                        {
                            var attArguments = instance
                                .GetAllNamedAttributeArgumentTuples()
                                .ToList();
                            return resolvedNamedArgumentList.All(attArguments.Contains);
                        })
                    )
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var argumentsDescriptions = matchingAttributeInstances.Select(instance =>
                            ruleType
                                .GetAllNamedAttributeArgumentTuples(instance.Type)
                                .FormatDescription(
                                    "without named arguments",
                                    "with named argument",
                                    "with named arguments",
                                    elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
                                )
                        );
                        var failDescription = argumentsDescriptions.FormatDescription(
                            $"does not have attribute \"{resolvedAttribute.FullName}\"",
                            $"does only have attribute \"{resolvedAttribute.FullName}\"",
                            $"does only have attribute \"{resolvedAttribute.FullName}\"",
                            elementDescription: str => str
                        );
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }
            var description = namedArgumentList.FormatDescription(
                $"have attribute \"{attributeFullName}\"",
                $"have attribute \"{attributeFullName}\" with named argument",
                $"have attribute \"{attributeFullName}\" with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameEquals(name),
                obj => "does have name " + obj.Name,
                $"have name \"{name}\""
            );
        }

        public static ICondition<TRuleType> HaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameMatches(pattern),
                obj => "does have name " + obj.Name,
                $"have name matching \"{pattern}\""
            );
        }

        public static ICondition<TRuleType> HaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameStartsWith(pattern),
                obj => "does have name " + obj.Name,
                "have name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameEndsWith(pattern),
                obj => "does have name " + obj.Name,
                "have name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameContains(pattern),
                obj => "does have name " + obj.Name,
                "have name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveFullName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameEquals(name),
                obj => "does have full name " + obj.FullName,
                "have full name \"" + name + "\""
            );
        }

        public static ICondition<TRuleType> HaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameMatches(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveFullNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameStartsWith(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveFullNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameEndsWith(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameContains(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static ICondition<TRuleType> HaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.AssemblyQualifiedNameMatches(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.AssemblyQualifiedNameStartsWith(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.AssemblyQualifiedNameEndsWith(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> HaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.AssemblyQualifiedNameContains(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "have assembly qualified name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> BePrivate()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == Private,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be private"
            );
        }

        public static ICondition<TRuleType> BePublic()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == Public,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be public"
            );
        }

        public static ICondition<TRuleType> BeProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == Protected,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be protected"
            );
        }

        public static ICondition<TRuleType> BeInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == Internal,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be internal"
            );
        }

        public static ICondition<TRuleType> BeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == ProtectedInternal,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be protected internal"
            );
        }

        public static ICondition<TRuleType> BePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == PrivateProtected,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be private protected"
            );
        }

        //Relation Conditions

        public static RelationCondition<TRuleType, IType> DependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                DependOnAny,
                "depend on any types that",
                "does not depend on any types that"
            );
        }

        public static RelationCondition<TRuleType, IType> OnlyDependOnTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                OnlyDependOn,
                "only depend on types that",
                "does not only depend on types that"
            );
        }

        public static RelationCondition<TRuleType, Attribute> HaveAnyAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(
                HaveAnyAttributes,
                "have attributes that",
                "does not have attributes that"
            );
        }

        public static RelationCondition<TRuleType, Attribute> OnlyHaveAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(
                OnlyHaveAttributes,
                "only have attributes that",
                "does not only have attributes that"
            );
        }

        //Negations

        public static ICondition<TRuleType> NotExist()
        {
            return new ExistsCondition<TRuleType>(false);
        }

        public static ICondition<TRuleType> NotBe(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var objectList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var failedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "is " + objectProvider.Description
                    );
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "not be any of no objects (always true)",
                "not be",
                "not be"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotCallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var methodList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var failedObjects = typeList
                    .Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var calledMethods = failedObject
                        .GetCalledMethods()
                        .Intersect(methodList)
                        .Select(method => $"\"{method.FullName}\"");
                    var dynamicFailDescription = "does call " + string.Join(" and ", calledMethods);
                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }
                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "not call any of no methods (always true)",
                "not call",
                "not call any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dependants = failedObject
                        .GetTypeDependencies()
                        .Intersect(typeList)
                        .Select(type => $"\"{type.FullName}\"");
                    var dynamicFailDescription =
                        "does depend on " + string.Join(" and ", dependants);
                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }
                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "not depend on any of no types (always true)",
                "not depend on",
                "not depend on any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Intersect(attributeList))
                    {
                        dynamicFailDescription += first
                            ? " " + attribute.FullName
                            : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = objectProvider.FormatDescription(
                "not have any of no attributes (always true)",
                "not have",
                "not have any"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType.GetAllAttributeArgumentValues().ToList();
                    if (!resolvedArgumentValueList.Any(attArguments.Contains))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var failedAttributesAndArguments = ruleType
                            .AttributeInstances.Select(instance =>
                            {
                                var attributeArguments = ruleType
                                    .GetAllAttributeArgumentValues(instance.Type)
                                    .ToList();
                                var failedArguments = resolvedArgumentValueList
                                    .Where(attributeArguments.Contains)
                                    .ToList();
                                return (instance, failedArguments);
                            })
                            .Where(t => t.failedArguments.Any())
                            .ToList();
                        var argumentDescriptions = failedAttributesAndArguments.Select(t =>
                        {
                            var withArguments =
                                t.failedArguments.Count == 1 ? "with argument" : "with arguments";
                            var arguments = t.failedArguments.Select(arg => $"\"{arg}\"");
                            var argumentsDescription = string.Join(" and ", arguments);
                            return $"attribute {t.instance.Type.FullName} {withArguments} {argumentsDescription}";
                        });
                        var failDescription =
                            "does have " + string.Join(" and ", argumentDescriptions);
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = argumentValueList.FormatDescription(
                "not have any attributes with any of no arguments (always true)",
                "not have any attributes with argument",
                "not have any attributes with arguments"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var failedArguments = ruleType
                        .GetAllAttributeArgumentValues(resolvedAttribute)
                        .Where(resolvedArgumentValueList.Contains)
                        .ToList();
                    if (!failedArguments.Any())
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var withArguments =
                            failedArguments.Count == 1 ? "with argument" : "with arguments";
                        var arguments = failedArguments.Select(arg => $"\"{arg}\"");
                        var argumentsDescription = string.Join(" and ", arguments);
                        var failDescription =
                            $"does have attribute \"{resolvedAttribute.FullName}\" {withArguments} {argumentsDescription}";
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = argumentValueList.FormatDescription(
                $"not have attribute \"{attributeFullName}\" with any of no arguments (always true)",
                $"not have attribute \"{attributeFullName}\" with argument",
                $"not have attribute \"{attributeFullName}\" with arguments"
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType.GetAllNamedAttributeArgumentTuples().ToList();
                    if (!resolvedNamedArgumentList.Any(attArguments.Contains))
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var failedAttributesAndArguments = ruleType
                            .AttributeInstances.Select(instance =>
                            {
                                var attributeArguments =
                                    ruleType.GetAllNamedAttributeArgumentTuples(instance.Type);
                                var failedArguments = resolvedNamedArgumentList
                                    .Where(attributeArguments.Contains)
                                    .ToList();
                                return (instance, failedArguments);
                            })
                            .Where(t => t.failedArguments.Any())
                            .ToList();
                        var argumentDescriptions = failedAttributesAndArguments.Select(t =>
                        {
                            var withArguments =
                                t.failedArguments.Count == 1
                                    ? "with named argument"
                                    : "with named arguments";
                            var arguments = t.failedArguments.Select(arg =>
                                $"\"{arg.Item1}={arg.Item2}\""
                            );
                            var argumentsDescription = string.Join(" and ", arguments);
                            return $"attribute {t.instance.Type.FullName} {withArguments} {argumentsDescription}";
                        });
                        var failDescription =
                            "does have " + string.Join(" and ", argumentDescriptions);
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }
            var description = namedArgumentList.FormatDescription(
                "not have any attributes with any of no named arguments (always true)",
                "not have any attributes with named argument",
                "not have any attributes with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(
            string attributeFullName,
            [NotNull] Func<Architecture, Attribute> getAttribute,
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> ruleTypes,
                Architecture architecture
            )
            {
                var resolvedAttribute = getAttribute(architecture);
                var resolvedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var failedArguments = ruleType
                        .GetAllNamedAttributeArgumentTuples(resolvedAttribute)
                        .Where(resolvedArgumentList.Contains)
                        .ToList();
                    if (!failedArguments.Any())
                    {
                        yield return new ConditionResult(ruleType, true);
                    }
                    else
                    {
                        var withArguments =
                            failedArguments.Count == 1
                                ? "with named argument"
                                : "with named arguments";
                        var arguments = failedArguments.Select(arg =>
                            $"\"{arg.Item1}={arg.Item2}\""
                        );
                        var argumentsDescription = string.Join(" and ", arguments);
                        var failDescription =
                            $"does have attribute \"{resolvedAttribute.FullName}\" {withArguments} {argumentsDescription}";
                        yield return new ConditionResult(ruleType, false, failDescription);
                    }
                }
            }

            var description = namedArgumentList.FormatDescription(
                $"not have attribute \"{attributeFullName}\" with any of no named arguments (always true)",
                $"not have attribute \"{attributeFullName}\" with named argument",
                $"not have attribute \"{attributeFullName}\" with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.NameEquals(name),
                obj => "does have name " + obj.Name,
                $"not have name \"{name}\""
            );
        }

        public static ICondition<TRuleType> NotHaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.NameMatches(pattern),
                obj => "does have name " + obj.Name,
                $"not have name matching \"{pattern}\""
            );
        }

        public static ICondition<TRuleType> NotHaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.NameStartsWith(pattern),
                obj => "does have name " + obj.Name,
                "not have name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.NameEndsWith(pattern),
                obj => "does have name " + obj.Name,
                "not have name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.NameContains(pattern),
                obj => "does have name " + obj.Name,
                "not have name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFullName(string fullName)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameEquals(fullName),
                obj => "does have full name " + obj.FullName,
                "not have full name \"" + fullName + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameMatches(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFullNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameStartsWith(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFullNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameEndsWith(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameContains(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveAssemblyQualifiedName(
            string assemblyQualifiedName
        )
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "not have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.AssemblyQualifiedNameMatches(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "not have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.AssemblyQualifiedNameStartsWith(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "not have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.AssemblyQualifiedNameEndsWith(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "not have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotHaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.AssemblyQualifiedNameContains(pattern),
                obj => "does have assembly qualified name " + obj.AssemblyQualifiedName,
                "not have assembly qualified name containing \"" + pattern + "\""
            );
        }

        public static ICondition<TRuleType> NotBePrivate()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != Private,
                "not be private",
                "is private"
            );
        }

        public static ICondition<TRuleType> NotBePublic()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != Public,
                "not be public",
                "is public"
            );
        }

        public static ICondition<TRuleType> NotBeProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != Protected,
                "not be protected",
                "is protected"
            );
        }

        public static ICondition<TRuleType> NotBeInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != Internal,
                "not be internal",
                "is internal"
            );
        }

        public static ICondition<TRuleType> NotBeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != ProtectedInternal,
                "not be protected internal",
                "is protected internal"
            );
        }

        public static ICondition<TRuleType> NotBePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != PrivateProtected,
                "not be private protected",
                "is private protected"
            );
        }

        //Relation Condition Negations

        public static RelationCondition<TRuleType, IType> NotDependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                NotDependOnAny,
                "not depend on any types that",
                "does depend on any types that"
            );
        }

        public static RelationCondition<TRuleType, Attribute> NotHaveAnyAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(
                NotHaveAnyAttributes,
                "not have attributes that",
                "does have attributes that"
            );
        }
    }
}
