using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public static class ObjectConditionsDefinition<TRuleType>
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

            return new ArchitectureCondition<TRuleType>(
                Condition,
                (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                    ? "not exist"
                    : "be " + objectProvider.Description
            );
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
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Except(methodList))
                    {
                        dynamicFailDescription += first
                            ? " " + method.FullName
                            : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }
                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description =
                (objectProvider as ISizedObjectProvider<MethodMember>)?.Count == 0
                    ? "call any of no methods (impossible)"
                    : "call any " + objectProvider.Description;
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

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description =
                (objectProvider as ISizedObjectProvider<IType>)?.Count == 0
                    ? "depend on any of no types (impossible)"
                    : "depend on any " + objectProvider.Description;
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

            var description =
                (objectProvider as ISizedObjectProvider<IType>)?.Count == 0
                    ? "have no dependencies"
                    : "only depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<Attribute>;
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
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                            ? "does not have any of no attributes (always true)"
                            : "does not have any " + objectProvider.Description
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description =
                (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                    ? "have one of no attributes (impossible)"
                    : "have any " + objectProvider.Description;
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

            var description =
                (objectProvider as ISizedObjectProvider<Attribute>)?.Count == 0
                    ? "have no attributes"
                    : "does only have " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return HaveAnyAttributesWithArguments(argumentValues);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have no or any attributes with arguments (always true)";
                failDescription = (ruleType, architecture) =>
                    "not have no or any attributes with arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(attribute => attribute != firstArgument)
                    .Aggregate(
                        "have any attributes with arguments \"" + firstArgument + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );

                failDescription = (ruleType, architecture) =>
                {
                    var actualArgumentValues = ruleType
                        .AttributeInstances.SelectMany(instance =>
                            instance.AttributeArguments.Select(argument => argument.Value)
                        )
                        .ToList();
                    if (!actualArgumentValues.Any())
                    {
                        return "does have no attribute with an argument";
                    }

                    var firstActualArgumentValue = actualArgumentValues.First();
                    return actualArgumentValues.Aggregate(
                        "does have attributes with argument values \""
                            + firstActualArgumentValue
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attributeArguments = obj
                    .AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.Select(arg => arg.Value)
                    )
                    .ToList();
                var typeAttributeArguments = attributeArguments
                    .OfType<ITypeInstance<IType>>()
                    .Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>())
                    .ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (!attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(
            [NotNull] Attribute attribute,
            IEnumerable<object> argumentValues
        )
        {
            string description,
                failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                failDescription = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does not have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance
                        .AttributeArguments.Select(arg => arg.Value)
                        .ToList();
                    var typeAttributeArguments = attributeArguments
                        .OfType<ITypeInstance<IType>>()
                        .Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>())
                        .ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute:
                    ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(
            [NotNull] Type attribute,
            IEnumerable<object> argumentValues
        )
        {
            string description,
                failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                failDescription = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does not have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return false;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance
                        .AttributeArguments.Select(arg => arg.Value)
                        .ToList();
                    var typeAttributeArguments = attributeArguments
                        .OfType<ITypeInstance<IType>>()
                        .Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>())
                        .ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute:
                    ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        )
        {
            var argumentList = attributeArguments.ToList();
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentList.IsNullOrEmpty())
            {
                description = "have no or any attributes with named arguments (always true)";
                failDescription = (ruleType, architecture) =>
                    "not have no or any attributes with named arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(attribute => attribute != firstArgument)
                    .Aggregate(
                        "have any attributes with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );

                failDescription = (ruleType, architecture) =>
                {
                    var actualNamedArguments = ruleType
                        .AttributeInstances.SelectMany(instance =>
                            instance.AttributeArguments.OfType<AttributeNamedArgument>()
                        )
                        .ToList();
                    if (!actualNamedArguments.Any())
                    {
                        return "does have no attribute with a named argument";
                    }

                    var firstActualNamedArgument = actualNamedArguments.First();
                    return actualNamedArguments.Aggregate(
                        "does have attributes with named arguments \""
                            + firstActualNamedArgument.Name
                            + "="
                            + firstActualNamedArgument.Value
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Name + "=" + arg.Value + "\""
                    );
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attArguments = obj
                    .AttributeInstances.SelectMany(instance =>
                        instance
                            .AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value))
                    )
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                    .ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (
                            typeAttributeArguments.All(t =>
                                t.Name != arg.Item1
                                || t.Value is ITypeInstance<IType> typeInstance
                                    && typeInstance.Type.FullName != argType.FullName
                                || t.Value is IType type && type.FullName != argType.FullName
                            )
                        )
                        {
                            return false;
                        }
                    }
                    else if (!attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(
            [NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            string description,
                failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
                failDescription = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does not have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance
                        .AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value))
                        .ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                        .ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (
                                typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1
                                    || t.Value is ITypeInstance<IType> typeInstance
                                        && typeInstance.Type.FullName != argType.FullName
                                    || t.Value is IType type && type.FullName != argType.FullName
                                )
                            )
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArgs.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute:
                    ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(
            [NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            string description,
                failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
                failDescription = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does not have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return false;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance
                        .AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value))
                        .ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                        .ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (
                                typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1
                                    || t.Value is ITypeInstance<IType> typeInstance
                                        && typeInstance.Type.FullName != argType.FullName
                                    || t.Value is IType type && type.FullName != argType.FullName
                                )
                            )
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArgs.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute:
                    ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
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

        public static ICondition<TRuleType> HaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameContains(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name containing \"" + pattern + "\""
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

            return new ArchitectureCondition<TRuleType>(
                Condition,
                (objectProvider as ISizedObjectProvider<ICanBeAnalyzed>)?.Count == 0
                    ? "exist"
                    : "not be " + objectProvider.Description
            );
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
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Intersect(methodList))
                    {
                        dynamicFailDescription += first
                            ? " " + method.FullName
                            : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description =
                (objectProvider as ISizedObjectProvider<MethodMember>)?.Count == 0
                    ? "not call any of no methods (always true)"
                    : "not call any " + objectProvider.Description;
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
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Intersect(typeList))
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

            var description =
                (objectProvider as ISizedObjectProvider<IType>)?.Count == 0
                    ? "not depend on any of no types (always true)"
                    : "not depend on any " + objectProvider.Description;
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

            var description =
                (objectProvider as ISizedObjectProvider<Attribute>)?.Count == 0
                    ? "not have any of no attributes (always true)"
                    : "not have any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAnyAttributesWithArguments(argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            var argumentValues = new List<object> { firstArgumentValue };
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            var attributeArguments = new List<(string, object)> { firstAttributeArgument };
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have no or any attributes with arguments (impossible)";
                failDescription = (ruleType, architecture) =>
                    "have no or any attributes with arguments (always)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(attribute => attribute != firstArgument)
                    .Aggregate(
                        "not have any attributes with arguments \"" + firstArgument + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );

                failDescription = (ruleType, architecture) =>
                {
                    var actualArgumentValues = ruleType
                        .AttributeInstances.SelectMany(instance =>
                            instance.AttributeArguments.Select(argument => argument.Value)
                        )
                        .ToList();
                    if (!actualArgumentValues.Any())
                    {
                        return "does have no attribute with an argument";
                    }

                    var firstActualArgumentValue = actualArgumentValues.First();
                    return actualArgumentValues.Aggregate(
                        "does have attributes with argument values \""
                            + firstActualArgumentValue
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attributeArguments = obj
                    .AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.Select(arg => arg.Value)
                    )
                    .ToList();
                var typeAttributeArguments = attributeArguments
                    .OfType<ITypeInstance<IType>>()
                    .Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>())
                    .ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.Any(t => t.FullName == argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(
            [NotNull] Attribute attribute,
            IEnumerable<object> argumentValues
        )
        {
            string description,
                failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "not have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                failDescription = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance
                        .AttributeArguments.Select(arg => arg.Value)
                        .ToList();
                    var typeAttributeArguments = attributeArguments
                        .OfType<ITypeInstance<IType>>()
                        .Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>())
                        .ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute:
                    ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(
            [NotNull] Type attribute,
            IEnumerable<object> argumentValues
        )
        {
            string description,
                failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "not have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
                failDescription = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does have attribute \""
                            + attribute.FullName
                            + "\" with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return true;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance
                        .AttributeArguments.Select(arg => arg.Value)
                        .ToList();
                    var typeAttributeArguments = attributeArguments
                        .OfType<ITypeInstance<IType>>()
                        .Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>())
                        .ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute:
                    ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        )
        {
            var argumentList = attributeArguments.ToList();
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have no or any attributes with named arguments (impossible)";
                failDescription = (ruleType, architecture) =>
                    "have no or any attributes with named arguments (always true)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(attribute => attribute != firstArgument)
                    .Aggregate(
                        "not have any attributes with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );

                failDescription = (ruleType, architecture) =>
                {
                    var actualNamedArguments = ruleType
                        .AttributeInstances.SelectMany(instance =>
                            instance.AttributeArguments.OfType<AttributeNamedArgument>()
                        )
                        .ToList();
                    if (!actualNamedArguments.Any())
                    {
                        return "does have no attribute with a named argument";
                    }

                    var firstActualNamedArgument = actualNamedArguments.First();
                    return actualNamedArguments.Aggregate(
                        "does have attributes with named arguments \""
                            + firstActualNamedArgument.Name
                            + "="
                            + firstActualNamedArgument.Value
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Name + "=" + arg.Value + "\""
                    );
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attArguments = obj
                    .AttributeInstances.SelectMany(instance =>
                        instance
                            .AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value))
                    )
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                    .ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (
                            typeAttributeArguments.Any(t =>
                                t.Name == arg.Item1
                                && (
                                    t.Value is ITypeInstance<IType> typeInstance
                                        && typeInstance.Type.FullName == argType.FullName
                                    || t.Value is IType type && type.FullName == argType.FullName
                                )
                            )
                        )
                        {
                            return false;
                        }
                    }
                    else if (attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(
            [NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            string description,
                failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "not have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
                failDescription = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance
                        .AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value))
                        .ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                        .ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (
                                typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1
                                    || t.Value is ITypeInstance<IType> typeInstance
                                        && typeInstance.Type.FullName != argType.FullName
                                    || t.Value is IType type && type.FullName != argType.FullName
                                )
                            )
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArgs.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute:
                    ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(
            [NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            string description,
                failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "not have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
                failDescription = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "does have attribute \""
                            + attribute.FullName
                            + "\" with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture)
                {
                    //can't have a dependency
                    return true;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance
                        .AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value))
                        .ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType)
                        .ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (
                                typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1
                                    || t.Value is ITypeInstance<IType> typeInstance
                                        && typeInstance.Type.FullName != argType.FullName
                                    || t.Value is IType type && type.FullName != argType.FullName
                                )
                            )
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArgs.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute:
                    ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
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

        public static ICondition<TRuleType> NotHaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameContains(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name containing \"" + pattern + "\""
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
