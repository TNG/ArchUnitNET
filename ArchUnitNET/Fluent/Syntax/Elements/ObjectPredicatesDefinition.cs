using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    internal static class ObjectPredicatesDefinition<T>
        where T : ICanBeAnalyzed
    {
        public static IPredicate<T> Are(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Intersect(objectProvider.GetObjects(architecture).OfType<T>());
            }

            return new ArchitecturePredicate<T>(
                Filter,
                (objectProvider as ISizedObjectProvider<ICanBeAnalyzed>)?.Count == 0
                    ? "are any of no objects (always empty)"
                    : "are " + objectProvider.Description
            );
        }

        public static IPredicate<T> CallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = objectProvider.CreateDynamicDescription(
                "do not call any method",
                "call",
                "call any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.GetTypeDependencies().Intersect(types).Any());
            }

            var description = objectProvider.CreateDynamicDescription(
                "depend on any of no types (impossible)",
                "depend on",
                "depend on any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> FollowCustomPredicate(
            Func<T, bool> predicate,
            string description
        )
        {
            return new SimplePredicate<T>(predicate, description);
        }

        public static IPredicate<T> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj =>
                    obj.GetTypeDependencies(architecture).Except(types).IsNullOrEmpty()
                );
            }

            var description = "only depend on " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => obj.Attributes.Intersect(attributes).Any());
            }

            var description = objectProvider.CreateDynamicDescription(
                "have any of no attributes (always empty)",
                "have attribute",
                "have any attribute"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> OnlyHaveAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var attributes = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Except(attributes).Any());
            }

            var description = "only have " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            string description;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have no or any attributes with arguments (always true)";
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
            }

            bool Predicate(T obj, Architecture architecture)
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

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithArguments(
            [NotNull] IObject<Attribute> attribute,
            IEnumerable<object> argumentValues
        )
        {
            string description;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> { null };
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute " + attribute.Description;
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute "
                            + attribute.Description
                            + " with arguments \""
                            + firstArgument
                            + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\""
                    );
            }

            bool Predicate(T obj, Architecture architecture)
            {
                var archUnitAttribute = attribute.Get(architecture);
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

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        )
        {
            var argumentList = attributeArguments.ToList();
            string description;
            if (argumentList.IsNullOrEmpty())
            {
                description = "have no or any attributes with named arguments (always true)";
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
            }

            bool Predicate(T obj, Architecture architecture)
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

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveAttributeWithNamedArguments(
            [NotNull] IObject<Attribute> attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            string description;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute " + attribute.Description;
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList
                    .Where(att => att != firstArgument)
                    .Aggregate(
                        "have attribute "
                            + attribute.Description
                            + " with named arguments \""
                            + firstArgument.Item1
                            + "="
                            + firstArgument.Item2
                            + "\"",
                        (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\""
                    );
            }

            bool Predicate(T obj, Architecture architecture)
            {
                var archUnitAttribute = attribute.Get(architecture);

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

            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> HaveName(string name)
        {
            return new SimplePredicate<T>(
                obj => obj.NameEquals(name),
                "have name \"" + name + "\""
            );
        }

        public static IPredicate<T> HaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameMatches(pattern),
                "have name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameEndsWith(pattern),
                "have name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.NameContains(pattern),
                "have name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullName(string fullName)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameEquals(fullName),
                "have full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> HaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameStartsWith(pattern),
                "have full name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameEndsWith(pattern),
                "have full name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.FullNameContains(pattern),
                "have full name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                "have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameMatches(pattern),
                "have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameStartsWith(pattern),
                "have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameEndsWith(pattern),
                "have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> HaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => obj.AssemblyQualifiedNameContains(pattern),
                "have assembly qualified name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> ArePrivate()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Private, "are private");
        }

        public static IPredicate<T> ArePublic()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Public, "are public");
        }

        public static IPredicate<T> AreProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Protected, "are protected");
        }

        public static IPredicate<T> AreInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility == Internal, "are internal");
        }

        public static IPredicate<T> AreProtectedInternal()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility == ProtectedInternal,
                "are protected internal"
            );
        }

        public static IPredicate<T> ArePrivateProtected()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility == PrivateProtected,
                "are private protected"
            );
        }

        //Negations

        public static IPredicate<T> AreNot(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                return objects.Except(objectProvider.GetObjects(architecture).OfType<T>());
            }

            return new ArchitecturePredicate<T>(
                Filter,
                (objectProvider as ISizedObjectProvider<ICanBeAnalyzed>)?.Count == 0
                    ? "are not any of no objects (always true)"
                    : "are not " + objectProvider.Description
            );
        }

        public static IPredicate<T> DoNotCallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.GetCalledMethods().Intersect(methods).Any());
            }

            var description = objectProvider.CreateDynamicDescription(
                "do not call any of no methods (always true)",
                "do not call",
                "do not call any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj =>
                    !obj.GetTypeDependencies(architecture).Intersect(types).Any()
                );
            }

            var description = objectProvider.CreateDynamicDescription(
                "do not depend on any of no types (always true)",
                "do not depend on",
                "do not depend on any"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributes(
            IObjectProvider<Attribute> objectProvider
        )
        {
            IEnumerable<T> Filter(IEnumerable<T> objects, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return objects.Where(obj => !obj.Attributes.Intersect(types).Any());
            }

            var description = objectProvider.CreateDynamicDescription(
                "do not have any of no attributes (always true)",
                "do not have attribute",
                "do not have any attribute"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();
            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attributeArguments = ruleType.GetAllAttributeArgumentValues().ToList();
                    if (!resolvedArgumentValueList.Any(attributeArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }
            var description = argumentValueList.CreateDynamicDescription(
                "do not have any attributes with any of no arguments (always true)",
                "do not have any attributes with argument",
                "do not have any attributes with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithArguments(
            [NotNull] IObject<Attribute> attribute,
            IEnumerable<object> argumentValues
        )
        {
            var argumentValueList = argumentValues as IList<object> ?? argumentValues.ToList();

            IEnumerable<T> Predicate(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = attribute.Get(architecture);
                var resolvedArgumentValueList = argumentValueList
                    .ResolveAttributeArguments(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attributeArguments = ruleType
                        .GetAllAttributeArgumentValues(resolvedAttribute)
                        .ToList();
                    if (!resolvedArgumentValueList.Any(attributeArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = argumentValueList.CreateDynamicDescription(
                $"do not have attribute {attribute.Description} with any of no arguments (always true)",
                $"do not have attribute {attribute.Description} with argument",
                $"do not have attribute {attribute.Description} with arguments"
            );
            return new ArchitecturePredicate<T>(Predicate, description);
        }

        public static IPredicate<T> DoNotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType.GetAllNamedAttributeArgumentTuples().ToList();
                    if (!resolvedNamedArgumentList.Any(attArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = namedArgumentList.CreateDynamicDescription(
                "do not have any attributes with any of no named arguments (always true)",
                "do not have any attributes with named argument",
                "do not have any attributes with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveAttributeWithNamedArguments(
            [NotNull] IObject<Attribute> attribute,
            IEnumerable<(string, object)> namedArguments
        )
        {
            var namedArgumentList =
                namedArguments as IList<(string, object)> ?? namedArguments.ToList();
            IEnumerable<T> Condition(IEnumerable<T> ruleTypes, Architecture architecture)
            {
                var resolvedAttribute = attribute.Get(architecture);
                var resolvedNamedArgumentList = namedArgumentList
                    .ResolveNamedAttributeArgumentTuples(architecture)
                    .ToList();
                foreach (var ruleType in ruleTypes)
                {
                    var attArguments = ruleType
                        .GetAllNamedAttributeArgumentTuples(resolvedAttribute)
                        .ToList();
                    if (!resolvedNamedArgumentList.Any(attArguments.Contains))
                    {
                        yield return ruleType;
                    }
                }
            }

            var description = namedArgumentList.CreateDynamicDescription(
                $"do not have attribute {attribute.Description} with any of no named arguments (always true)",
                $"do not have attribute {attribute.Description} with named argument",
                $"do not have attribute {attribute.Description} with named arguments",
                elementDescription: arg => $"\"{arg.Item1}={arg.Item2}\""
            );
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveName(string name)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameEquals(name),
                $"do not have name \"{name}\""
            );
        }

        public static IPredicate<T> DoNotHaveNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameMatches(pattern),
                $"do not have name matching \"{pattern}\""
            );
        }

        public static IPredicate<T> DoNotHaveNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameStartsWith(pattern),
                "do not have name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameEndsWith(pattern),
                "do not have name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.NameContains(pattern),
                "do not have name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullName(string fullName)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameEquals(fullName),
                "do not have full name \"" + fullName + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameMatches(pattern),
                "do not have full name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameStartsWith(pattern),
                "do not have full name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameEndsWith(pattern),
                "do not have full name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.FullNameContains(pattern),
                "do not have full name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameEquals(assemblyQualifiedName),
                "do not have assembly qualified name \"" + assemblyQualifiedName + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameMatches(pattern),
                "do not have assembly qualified name matching \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameStartingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameStartsWith(pattern),
                "do not have assembly qualified name starting with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameEndingWith(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameEndsWith(pattern),
                "do not have assembly qualified name ending with \"" + pattern + "\""
            );
        }

        public static IPredicate<T> DoNotHaveAssemblyQualifiedNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                obj => !obj.AssemblyQualifiedNameContains(pattern),
                "do not have assembly qualified name containing \"" + pattern + "\""
            );
        }

        public static IPredicate<T> AreNotPrivate()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Private, "are not private");
        }

        public static IPredicate<T> AreNotPublic()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Public, "are not public");
        }

        public static IPredicate<T> AreNotProtected()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Protected, "are not protected");
        }

        public static IPredicate<T> AreNotInternal()
        {
            return new SimplePredicate<T>(obj => obj.Visibility != Internal, "are not internal");
        }

        public static IPredicate<T> AreNotProtectedInternal()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility != ProtectedInternal,
                "are not protected internal"
            );
        }

        public static IPredicate<T> AreNotPrivateProtected()
        {
            return new SimplePredicate<T>(
                obj => obj.Visibility != PrivateProtected,
                "are not private protected"
            );
        }
    }
}
